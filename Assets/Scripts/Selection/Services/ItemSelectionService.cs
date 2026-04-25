using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Serving;
using System;

namespace Assets.Scripts.Selection
{
    public class ItemSelectionService : IItemSelectionService
    {
        private readonly IServedMenuItemFactory _menuItemFactory;

        public event EventHandler OnSelectionOpened;
        public event EventHandler OnSelectionClosed;

        private bool _isOpen;
        public bool IsOpen => _isOpen;

        public ItemSelectionMode CurrentMode { get; private set; }

        private ObjectHolder _targetHolder;
        public ObjectHolder CurrentTargetHolder => _targetHolder;

        public ItemSelectionService(IServedMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
        public void OpenDrinksSelection(ObjectHolder targetHolder)
        {
            if (_isOpen)
                return;
            if (targetHolder == null)
                return;

            _targetHolder = targetHolder;
            _isOpen = true;
            CurrentMode = ItemSelectionMode.Drinks;
            OnSelectionOpened?.Invoke(this, EventArgs.Empty);
        }

        public void OpenIngredientsSelection(ObjectHolder targetHolder)
        {
            if (_isOpen)
                return;
            if (targetHolder == null)
                return;

            _targetHolder = targetHolder;
            _isOpen = true;
            CurrentMode = ItemSelectionMode.Ingredients;
            OnSelectionOpened?.Invoke(this, EventArgs.Empty);

        }

        public void CloseSelection()
        {
            if (!_isOpen)
                return;

            _isOpen = false;
            _targetHolder = null;
            CurrentMode = ItemSelectionMode.None;
            OnSelectionClosed?.Invoke(this, EventArgs.Empty);
        }

        public bool TryConfirmSelection(KitchenObjectSo kitchenObjectSo)
        {
            if (!_isOpen || CurrentMode != ItemSelectionMode.Ingredients)
                return false;

            if (kitchenObjectSo == null || kitchenObjectSo.prefab == null || CurrentTargetHolder == null)
                return false;

            if (CurrentTargetHolder.HasObject)
                return false;

            CurrentTargetHolder.SpawnAndSet(kitchenObjectSo.prefab);

            CloseSelection();
            return true;
        }

        public bool TryConfirmSelection(MenuItemDefinitionSo menuItem)
        {
            if (!_isOpen || CurrentMode != ItemSelectionMode.Drinks)
                return false;

            if (menuItem == null || CurrentTargetHolder == null)
                return false;

            if (CurrentTargetHolder.HasObject)
                return false;
            if (!_menuItemFactory.TryCreate(menuItem, CurrentTargetHolder, out _))
                return false;

            CloseSelection();
            return true;
        }
    }
}
