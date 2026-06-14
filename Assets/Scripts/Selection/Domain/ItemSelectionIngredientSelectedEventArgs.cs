using OrderRushKitchen.KitchenObjects;
using System;

namespace OrderRushKitchen.Selection
{
    public class ItemSelectionIngredientSelectedEventArgs : EventArgs
    {
        public KitchenObjectSo Ingredient { get; }

        public ItemSelectionIngredientSelectedEventArgs(KitchenObjectSo ingredient)
        {
            Ingredient = ingredient;
        }
    }
}
