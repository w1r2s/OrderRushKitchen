using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;
using System;

namespace OrderRushKitchen.Selection
{
    public interface IItemSelectionService
    {
        void OpenIngredientsSelection(ObjectHolder targetHolder, IReadOnlyList<KitchenObjectSo> options);
        void OpenDrinksSelection(ObjectHolder targetHolder, IReadOnlyList<MenuItemDefinitionSo> options);
        void CloseSelection();
        bool TryConfirmSelection(KitchenObjectSo kitchenObjectSo);
        bool TryConfirmSelection(MenuItemDefinitionSo menuItem);

        bool IsOpen { get; }
        ItemSelectionMode CurrentMode { get; }
        ObjectHolder CurrentTargetHolder { get; }
        IReadOnlyList<MenuItemDefinitionSo> CurrentDrinkOptions { get; }
        IReadOnlyList<KitchenObjectSo> CurrentIngredientOptions { get; }

        event EventHandler OnSelectionOpened;
        event EventHandler OnSelectionClosed;
    }
}