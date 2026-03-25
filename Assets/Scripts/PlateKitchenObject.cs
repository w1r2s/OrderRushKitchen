using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSo kitchenObjectSo;
    }

    [SerializeField] private List<KitchenObjectSo> validKitchenObjcetSoList;

    private List<KitchenObjectSo> kitchenObjectSoList;

    private void Awake()
    {
        kitchenObjectSoList = new List<KitchenObjectSo>();
    }
    public bool TryAddIngredient(KitchenObjectSo kitchenObjectSo)
    {
        if (!validKitchenObjcetSoList.Contains(kitchenObjectSo))
        {
            return false;
        }
        if (kitchenObjectSoList.Contains(kitchenObjectSo))
        {
            return false;
        }
        else
        {
            kitchenObjectSoList.Add(kitchenObjectSo);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSo = kitchenObjectSo });

            return true;
        }
    }
    public List<KitchenObjectSo> GetKitchenObjectSoList()
    {
        return kitchenObjectSoList;
    }
}
