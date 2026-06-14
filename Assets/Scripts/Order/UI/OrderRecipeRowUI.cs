using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace OrderRushKitchen.Order
{
    public class OrderRecipeRowUI : MonoBehaviour
    {
        [SerializeField] private Image dishIconImage;
        [SerializeField] private Transform ingredientsRow;
        [SerializeField] private Image ingredientIconTemplate;

        private readonly List<Image> _ingredientIcons = new();

        public void Bind(OrderItem item)
        {
            ClearIngredientIcons();

            if (ingredientIconTemplate != null)
            {
                ingredientIconTemplate.gameObject.SetActive(false);
            }
            if (item == null || item.MenuItem == null)
                return;

            if (dishIconImage != null && item.MenuItem.icon != null)
            {
                dishIconImage.sprite = item.MenuItem.icon;
            }

            BuildIngredients(item);
        }
        private void BuildIngredients(OrderItem item)
        {
            if (item == null || item.MenuItem == null || ingredientsRow == null || ingredientIconTemplate == null)
                return;

            var ingredients = item.MenuItem.requiredIngredients;
            if (ingredients == null || ingredients.Count == 0)
                return;

            for (int i = 0; i < ingredients.Count; i++)
            {
                var ingredient = ingredients[i];
                if (ingredient == null || ingredient.sprite == null)
                    continue;

                var icon = Instantiate(ingredientIconTemplate, ingredientsRow, false);
                icon.gameObject.SetActive(true);
                icon.sprite = ingredient.sprite;
                _ingredientIcons.Add(icon);
            }
        }

        private void ClearIngredientIcons()
        {
            for (int i = 0; i < _ingredientIcons.Count; i++)
            {
                if (_ingredientIcons[i] != null)
                    Destroy(_ingredientIcons[i].gameObject);
            }

            _ingredientIcons.Clear();
        }
    }
}
