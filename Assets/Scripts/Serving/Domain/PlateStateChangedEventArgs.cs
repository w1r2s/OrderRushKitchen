using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System;

namespace OrderRushKitchen.Serving
{
    public class PlateStateChangedEventArgs : EventArgs
    {
        public PlateState PlateState { get; }
        public MenuItemDefinitionSo ResolvedMenuItem { get; }
        public PlateStateChangedEventArgs(PlateState plateState, MenuItemDefinitionSo menuItem)
        {
            PlateState = plateState;
            ResolvedMenuItem = menuItem;
        }
    }
}