using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Selection.UI
{
    public class ItemSelectionButtonUI : MonoBehaviour
    {

        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        private IItemSelectionService _selectionService;
        private KitchenObjectSo _ingredient;
        private MenuItemDefinitionSo _menuItem;

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }
        public void SetupIngredient(KitchenObjectSo ingredient, IItemSelectionService selectionService)
        {
            if (ingredient == null || selectionService == null)
                return;

            button.onClick.RemoveListener(OnClick);
            _ingredient = null;
            _menuItem = null;

            _ingredient = ingredient;
            _selectionService = selectionService;

            iconImage.sprite = _ingredient.sprite;
            button.onClick.AddListener(OnClick);
        }
        public void SetupDrink(MenuItemDefinitionSo menuItem, IItemSelectionService selectionService)
        {
            if (menuItem == null || selectionService == null)
                return;

            button.onClick.RemoveListener(OnClick);
            _ingredient = null;
            _menuItem = null;

            _menuItem = menuItem;
            _selectionService = selectionService;

            iconImage.sprite = menuItem.icon;
            button.onClick.AddListener(OnClick);
        }
        private void OnClick()
        {
            if (_ingredient != null)
            {
                _selectionService.TryConfirmSelection(_ingredient);
            }
            else if (_menuItem != null)
            {
                _selectionService.TryConfirmSelection(_menuItem);
            }
        }
    }
}
