using Assets.Scripts.ScriptableObjects;
using System;

namespace Assets.Scripts.Selection
{
    public class ItemSelectionDrinkSelectedEventArgs : EventArgs
    {
        public MenuItemDefinitionSo Drink { get; }

        public ItemSelectionDrinkSelectedEventArgs(MenuItemDefinitionSo drink)
        {
            Drink = drink;
        }
    }
}
