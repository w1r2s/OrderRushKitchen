using Assets.Scripts.Runtime;
using System;
using UnityEngine;

public abstract class ObjectHolder : MonoBehaviour, ILevelResettable
{
    [SerializeField] private Transform holdPoint;

    private KitchenObject _object;

    public event EventHandler OnObjectChanged;

    public bool HasObject => _object != null;

    public KitchenObject GetObject() => _object;

    public virtual bool CanAccept(KitchenObject obj)
    {
        return obj != null && !HasObject;
    }

    public virtual void SetObject(KitchenObject obj)
    {
        if (obj == null)
        {
            Debug.LogError($"{name}: SetObject called with null");
            return;
        }

        if (HasObject)
        {
            Debug.LogError($"{name}: holder already contains object");
            return;
        }

        _object = obj;
        obj.SetHolder(this);

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;

        OnObjectChanged?.Invoke(this, EventArgs.Empty);
    }

    public virtual KitchenObject RemoveObject()
    {
        KitchenObject obj = _object;
        _object = null;

        if (obj != null)
        {
            obj.ClearHolder();
        }

        OnObjectChanged?.Invoke(this, EventArgs.Empty);

        return obj;
    }
    public void SpawnAndSet(KitchenObject prefab)
    {
        var obj = Instantiate(prefab);
        SetObject(obj);
    }
    public bool TryGetObjectAs<T>(out T value) where T : KitchenObject
    {
        value = _object as T;
        return value != null;
    }

    public virtual void ResetForLevelTransition()
    {
        var obj = RemoveObject();

        if (obj != null)
            Destroy(obj.gameObject);
    }

}