using Assets.Scripts.ScriptableObjects;
using System;

namespace Assets.Scripts.Serving
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