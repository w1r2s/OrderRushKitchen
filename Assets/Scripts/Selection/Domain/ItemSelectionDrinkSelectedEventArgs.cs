using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System;

namespace OrderRushKitchen.Selection
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
