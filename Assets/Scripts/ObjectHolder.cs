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

    public bool TrySetObject(KitchenObject obj)
    {
        if (obj == null || obj.Holder != null || !CanAccept(obj))
            return false;

        AttachObject(obj);
        NotifyObjectChanged();
        OnObjectReceived(obj);
        return true;
    }

    public KitchenObject RemoveObject()
    {
        KitchenObject obj = DetachObject();

        if (obj != null)
        {
            NotifyObjectChanged();
        }

        return obj;
    }

    public bool TryTransferObjectTo(ObjectHolder target)
    {
        if (target == null || target == this || !HasObject)
            return false;

        KitchenObject obj = _object;

        if (obj.Holder != this || !target.CanAccept(obj))
            return false;

        DetachObject();
        target.AttachObject(obj);

        NotifyObjectChanged();
        target.NotifyObjectChanged();
        target.OnObjectReceived(obj);

        return true;
    }

    public bool TryRemoveAndDestroyObject()
    {
        KitchenObject obj = RemoveObject();

        if (obj == null)
            return false;

        Destroy(obj.gameObject);
        return true;
    }

    public bool TrySpawnAndSet(KitchenObject prefab, out KitchenObject spawnedObject)
    {
        spawnedObject = null;

        if (prefab == null || !CanAccept(prefab))
            return false;

        KitchenObject instance = Instantiate(prefab);

        if (!TrySetObject(instance))
        {
            Destroy(instance.gameObject);
            return false;
        }

        spawnedObject = instance;
        return true;
    }

    public bool TryGetObjectAs<T>(out T value) where T : KitchenObject
    {
        value = _object as T;
        return value != null;
    }

    public virtual void ResetForLevelTransition()
    {
        TryRemoveAndDestroyObject();
    }

    protected virtual void OnObjectReceived(KitchenObject obj)
    {
    }

    private void AttachObject(KitchenObject obj)
    {
        _object = obj;
        obj.SetHolder(this);

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
    }

    private KitchenObject DetachObject()
    {
        KitchenObject obj = _object;
        _object = null;

        if (obj != null)
        {
            obj.ClearHolder();
        }

        return obj;
    }

    private void NotifyObjectChanged()
    {
        OnObjectChanged?.Invoke(this, EventArgs.Empty);
    }
}
