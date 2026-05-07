using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Serving
{
    public class PlateKitchenObject : KitchenObject, ISubmittableMenuItemSource
    {
        public event EventHandler<IngredientsChangedEventArgs> OnIngredientsChanged;
        public event EventHandler<PlateStateChangedEventArgs> OnPlateStateChanged;

        [SerializeField] private List<KitchenObjectSo> availableIngredients;

        private List<KitchenObjectSo> kitchenObjectSoList;

        public PlateState State => isServed ? PlateState.Served : PlateState.Assembly;
        private bool isServed;

        public MenuItemDefinitionSo ResolvedMenuItem { get; private set; }

        private void Awake()
        {
            kitchenObjectSoList = new List<KitchenObjectSo>();
        }
        public bool TryAddIngredient(KitchenObjectSo kitchenObjectSo)
        {
            if (isServed)
            {
                return false;
            }
            if (kitchenObjectSo == null)
            {
                return false;
            }
            if (!availableIngredients.Contains(kitchenObjectSo))
            {
                return false;
            }
            else
            {
                kitchenObjectSoList.Add(kitchenObjectSo);

                OnIngredientsChanged?.Invoke(this, new IngredientsChangedEventArgs(new List<KitchenObjectSo>(kitchenObjectSoList)));

                return true;
            }
        }

        public bool TryServe(MenuItemDefinitionSo menuItem)
        {
            if (menuItem == null)
                return false;

            if (isServed)
                return false;

            ResolvedMenuItem = menuItem;
            isServed = true;
            OnPlateStateChanged?.Invoke(this, new PlateStateChangedEventArgs(State, ResolvedMenuItem));

            return true;
        }
        public IReadOnlyList<KitchenObjectSo> GetKitchenObjectSoList()
        {
            return kitchenObjectSoList;
        }

        public bool TryGetMenuItemForSubmit(out MenuItemDefinitionSo menuItem)
        {
            menuItem = null;
            if (State != PlateState.Served)
                return false;

            if (ResolvedMenuItem == null)
                return false;

            menuItem = ResolvedMenuItem;
            return true;
        }
    }
}