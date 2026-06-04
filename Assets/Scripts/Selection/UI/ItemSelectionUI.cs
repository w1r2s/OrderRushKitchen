using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Selection
{
    public class ItemSelectionUI : MonoBehaviour
    {
        public event EventHandler CloseRequested;
        public event EventHandler<ItemSelectionIngredientSelectedEventArgs> IngredientSelected;
        public event EventHandler<ItemSelectionDrinkSelectedEventArgs> DrinkSelected;

        [Header("Root")]
        [SerializeField] private GameObject selectionPanel;
        [SerializeField] private GameObject dimmer;
        [SerializeField] private Image selectionPanelImage;

        [Header("Backgrounds")]
        [SerializeField] private Sprite ingredientsBackground;
        [SerializeField] private Sprite drinksBackground;

        [Header("Layout")]
        [SerializeField] private Transform ingredientsShelvesArea;
        [SerializeField] private Transform drinksShelvesArea;
        [SerializeField] private RectTransform ingredientsShelfRowTemplate;
        [SerializeField] private RectTransform drinksShelfRowTemplate;
        [SerializeField] private ItemSelectionButtonUI itemButtonTemplate;
        [SerializeField] private Button closeButton;
        [SerializeField] private int itemsPerRow = 3;

        private readonly List<GameObject> _spawnedRows = new();

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
        }

        private void OnDestroy()
        {
            if (closeButton != null)
                closeButton.onClick.RemoveListener(OnCloseClicked);

            ClearRows();
        }

        public void InitializeHidden()
        {
            if (ingredientsShelfRowTemplate != null)
                ingredientsShelfRowTemplate.gameObject.SetActive(false);

            if (drinksShelfRowTemplate != null)
                drinksShelfRowTemplate.gameObject.SetActive(false);

            if (itemButtonTemplate != null)
                itemButtonTemplate.gameObject.SetActive(false);

            SetActiveShelvesArea(false, false);
            SetVisible(false);
        }

        public void ShowIngredients(IReadOnlyList<KitchenObjectSo> options)
        {
            ClearRows();
            ApplyBackground(ingredientsBackground);
            SetActiveShelvesArea(true, false);
            BuildIngredientRows(options);
            SetVisible(true);
        }

        public void ShowDrinks(IReadOnlyList<MenuItemDefinitionSo> options)
        {
            ClearRows();
            ApplyBackground(drinksBackground);
            SetActiveShelvesArea(false, true);
            BuildDrinkRows(options);
            SetVisible(true);
        }

        public void Hide()
        {
            ClearRows();
            SetActiveShelvesArea(false, false);
            SetVisible(false);
        }

        private void BuildIngredientRows(IReadOnlyList<KitchenObjectSo> options)
        {
            if (options == null || options.Count == 0)
                return;

            RectTransform currentRow = null;
            var perRow = Mathf.Max(1, itemsPerRow);
            var added = 0;

            for (int i = 0; i < options.Count; i++)
            {
                var item = options[i];
                if (item == null)
                    continue;

                if (added % perRow == 0)
                {
                    currentRow = CreateRow(ingredientsShelvesArea, ingredientsShelfRowTemplate);
                    if (currentRow == null)
                        return;
                }

                var button = CreateButton(currentRow);
                if (button == null)
                    return;

                button.SetupIngredient(item, OnIngredientSelected);
                added++;
            }
        }

        private void BuildDrinkRows(IReadOnlyList<MenuItemDefinitionSo> options)
        {
            if (options == null || options.Count == 0)
                return;

            RectTransform currentRow = null;
            var perRow = Mathf.Max(1, itemsPerRow);
            var added = 0;

            for (int i = 0; i < options.Count; i++)
            {
                var item = options[i];
                if (item == null || item.category != MenuItemCategory.Drink)
                    continue;

                if (added % perRow == 0)
                {
                    currentRow = CreateRow(drinksShelvesArea, drinksShelfRowTemplate);
                    if (currentRow == null)
                        return;
                }

                var button = CreateButton(currentRow);
                if (button == null)
                    return;

                button.SetupDrink(item, OnDrinkSelected);
                added++;
            }
        }

        private RectTransform CreateRow(Transform parent, RectTransform template)
        {
            if (parent == null || template == null)
                return null;

            var row = Instantiate(template, parent, false);
            row.gameObject.SetActive(true);
            _spawnedRows.Add(row.gameObject);

            return row;
        }

        private ItemSelectionButtonUI CreateButton(Transform parent)
        {
            if (parent == null || itemButtonTemplate == null)
                return null;

            var button = Instantiate(itemButtonTemplate, parent, false);
            button.gameObject.SetActive(true);

            return button;
        }

        private void ClearRows()
        {
            for (int i = 0; i < _spawnedRows.Count; i++)
            {
                if (_spawnedRows[i] != null)
                    Destroy(_spawnedRows[i]);
            }

            _spawnedRows.Clear();
        }

        private void ApplyBackground(Sprite background)
        {
            if (selectionPanelImage != null)
                selectionPanelImage.sprite = background;
        }

        private void SetActiveShelvesArea(bool ingredientsActive, bool drinksActive)
        {
            if (ingredientsShelvesArea != null)
                ingredientsShelvesArea.gameObject.SetActive(ingredientsActive);

            if (drinksShelvesArea != null)
                drinksShelvesArea.gameObject.SetActive(drinksActive);
        }

        private void SetVisible(bool visible)
        {
            if (dimmer != null)
                dimmer.SetActive(visible);

            if (selectionPanel != null)
                selectionPanel.SetActive(visible);
        }

        private void OnCloseClicked()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnIngredientSelected(KitchenObjectSo ingredient)
        {
            if (ingredient == null)
                return;

            IngredientSelected?.Invoke(this, new ItemSelectionIngredientSelectedEventArgs(ingredient));
        }

        private void OnDrinkSelected(MenuItemDefinitionSo drink)
        {
            if (drink == null)
                return;

            DrinkSelected?.Invoke(this, new ItemSelectionDrinkSelectedEventArgs(drink));
        }
    }
}