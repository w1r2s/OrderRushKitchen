using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace OrderRushKitchen.Selection
{
    public class ItemSelectionButtonUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;

        private KitchenObjectSo _ingredient;
        private MenuItemDefinitionSo _drink;
        private Action<KitchenObjectSo> _ingredientSelected;
        private Action<MenuItemDefinitionSo> _drinkSelected;

        private void Awake()
        {
            if (button != null)
                button.onClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClicked);
        }

        public void SetupIngredient(KitchenObjectSo ingredient, Action<KitchenObjectSo> selected)
        {
            _ingredient = ingredient;
            _drink = null;
            _ingredientSelected = selected;
            _drinkSelected = null;

            if (iconImage != null)
                iconImage.sprite = ingredient != null ? ingredient.sprite : null;
        }

        public void SetupDrink(MenuItemDefinitionSo drink, Action<MenuItemDefinitionSo> selected)
        {
            _ingredient = null;
            _drink = drink;
            _ingredientSelected = null;
            _drinkSelected = selected;

            if (iconImage != null)
                iconImage.sprite = drink != null ? drink.icon : null;
        }

        private void OnClicked()
        {
            if (_ingredient != null)
            {
                _ingredientSelected?.Invoke(_ingredient);
                return;
            }

            if (_drink != null)
                _drinkSelected?.Invoke(_drink);
        }
    }
}