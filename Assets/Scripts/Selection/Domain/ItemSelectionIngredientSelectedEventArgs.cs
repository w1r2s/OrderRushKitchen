using System;

namespace Assets.Scripts.Selection
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
