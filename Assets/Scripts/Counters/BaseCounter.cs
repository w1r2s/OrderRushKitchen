using System;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler OnObjectPlaced;

    [SerializeField] private Transform counterTopPoint;

    protected KitchenObject kitchenObject;

    public abstract void Interact(Player player);
    public virtual void InteractAlternate(Player player) { }

    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    public void SetKitchenObjcet(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() => kitchenObject;

    public void ClearKitchenObject() => kitchenObject = null;

    public bool HasKitchenObject() => kitchenObject != null;
}