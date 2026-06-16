using OrderRushKitchen.Composition;
using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace OrderRushKitchen.Serving
{
    public class ServedMenuItemKitchenObject : KitchenObject, ISubmittableMenuItemSource, IIngredientCompositionSource
    {
        public MenuItemDefinitionSo ServedMenuItem { get; private set; }

        public IReadOnlyList<KitchenObjectSo> Ingredients =>
            ServedMenuItem != null && ServedMenuItem.requiredIngredients != null
            ? ServedMenuItem.requiredIngredients
            : Array.Empty<KitchenObjectSo>();


        [SerializeField] private Transform contentRoot;

        public event EventHandler OnIngredientsChanged;

        public bool TryInitialize(MenuItemDefinitionSo menuItem)
        {
            if (menuItem == null)
            {
                return false;
            }

            if (ServedMenuItem != null)
            {
                return false;
            }

            if (menuItem.servedVisualPrefab == null || contentRoot == null)
            {
                return false;
            }
            ServedMenuItem = menuItem;
            Instantiate(ServedMenuItem.servedVisualPrefab, contentRoot, false);

            OnIngredientsChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public bool TryGetMenuItemForSubmit(out MenuItemDefinitionSo menuItem)
        {
            menuItem = null;
            if (ServedMenuItem == null)
                return false;

            menuItem = ServedMenuItem;
            return true;
        }
    }
}
