using Assets.Scripts.ScriptableObjects;
using System;

namespace Assets.Scripts.Selection
{
    public interface IItemSelectionService
    {
        void OpenIngredientsSelection(ObjectHolder targetHolder);
        void OpenDrinksSelection(ObjectHolder targetHolder);
        void CloseSelection();
        bool TryConfirmSelection(KitchenObjectSo kitchenObjectSo);
        bool TryConfirmSelection(MenuItemDefinitionSo menuItem);

        bool IsOpen { get; }
        ItemSelectionMode CurrentMode { get; }
        ObjectHolder CurrentTargetHolder { get; }

        event EventHandler OnSelectionOpened;
        event EventHandler OnSelectionClosed;
    }
}