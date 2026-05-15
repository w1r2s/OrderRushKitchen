using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Selection.UI
{
    public class ItemSelectionUI : MonoBehaviour
    {
        private IItemSelectionService _selectionService;

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

        [Inject]
        private void Construct(IItemSelectionService selectionService)
        {
            _selectionService = selectionService;
        }

        private void Start()
        {
            _selectionService.OnSelectionOpened += SelectionService_OnSelectionOpened;
            _selectionService.OnSelectionClosed += SelectionService_OnSelectionClosed;

            closeButton.onClick.AddListener(Cancel);

            ingredientsShelfRowTemplate.gameObject.SetActive(false);
            drinksShelfRowTemplate.gameObject.SetActive(false);

            SetActiveShelvesArea(false, false);
            Hide();
        }

        private void OnDestroy()
        {
            if (_selectionService != null)
            {
                _selectionService.OnSelectionOpened -= SelectionService_OnSelectionOpened;
                _selectionService.OnSelectionClosed -= SelectionService_OnSelectionClosed;
            }

            if (closeButton != null)
                closeButton.onClick.RemoveListener(Cancel);
        }

        private void SelectionService_OnSelectionOpened(object sender, EventArgs e)
        {
            ClearRows();
            ApplyBackground();
            ApplyShelvesArea();

            if (_selectionService.CurrentMode == ItemSelectionMode.Ingredients)
            {
                BuildIngredientRows();
            }
            else if (_selectionService.CurrentMode == ItemSelectionMode.Drinks)
            {
                BuildMenuItemRows();
            }

            Show();
        }

        private void SelectionService_OnSelectionClosed(object sender, EventArgs e)
        {
            ClearRows();
            SetActiveShelvesArea(false, false);
            Hide();
        }

        private void ApplyBackground()
        {
            if (selectionPanelImage == null)
                return;

            selectionPanelImage.sprite = _selectionService.CurrentMode switch
            {
                ItemSelectionMode.Ingredients => ingredientsBackground,
                ItemSelectionMode.Drinks => drinksBackground,
                _ => null
            };
        }

        private void ApplyShelvesArea()
        {
            SetActiveShelvesArea(
                _selectionService.CurrentMode == ItemSelectionMode.Ingredients,
                _selectionService.CurrentMode == ItemSelectionMode.Drinks
            );
        }

        private void SetActiveShelvesArea(bool ingredientsActive, bool drinksActive)
        {
            if (ingredientsShelvesArea != null)
                ingredientsShelvesArea.gameObject.SetActive(ingredientsActive);

            if (drinksShelvesArea != null)
                drinksShelvesArea.gameObject.SetActive(drinksActive);
        }

        private Transform GetCurrentShelvesArea()
        {
            return _selectionService.CurrentMode == ItemSelectionMode.Drinks
                ? drinksShelvesArea
                : ingredientsShelvesArea;
        }
        private RectTransform GetCurrentShelfRowTemplate()
        {
            return _selectionService.CurrentMode == ItemSelectionMode.Drinks
                ? drinksShelfRowTemplate
                : ingredientsShelfRowTemplate;
        }


        private void Cancel()
        {
            _selectionService.CloseSelection();
        }

        private void Show()
        {
            dimmer.SetActive(true);
            selectionPanel.SetActive(true);
        }

        private void Hide()
        {
            dimmer.SetActive(false);
            selectionPanel.SetActive(false);
        }

        private RectTransform CreateRow()
        {
            var parent = GetCurrentShelvesArea();
            var template = GetCurrentShelfRowTemplate();

            if (parent == null || template == null)
                return null;

            var row = Instantiate(template, parent, false);
            row.gameObject.SetActive(true);
            _spawnedRows.Add(row.gameObject);

            return row;
        }


        private void ClearRows()
        {
            foreach (var row in _spawnedRows)
            {
                Destroy(row);
            }

            _spawnedRows.Clear();
        }

        private void BuildIngredientRows()
        {
            var options = _selectionService.CurrentIngredientOptions;
            if (options == null || options.Count == 0)
                return;

            RectTransform currentRow = null;
            int perRow = Mathf.Max(1, itemsPerRow);
            int added = 0;

            for (int i = 0; i < options.Count; i++)
            {
                var item = options[i];
                if (item == null)
                    continue;

                if (added % perRow == 0)
                {
                    currentRow = CreateRow();
                    if (currentRow == null)
                        return;
                }

                var button = Instantiate(itemButtonTemplate, currentRow, false);
                button.gameObject.SetActive(true);
                button.SetupIngredient(item, _selectionService);
                added++;
            }
        }

        private void BuildMenuItemRows()
        {
            var options = _selectionService.CurrentDrinkOptions;
            if (options == null || options.Count == 0)
                return;

            RectTransform currentRow = null;
            int perRow = Mathf.Max(1, itemsPerRow);
            int added = 0;

            for (int i = 0; i < options.Count; i++)
            {
                var item = options[i];
                if (item == null || item.category != MenuItemCategory.Drink)
                    continue;

                if (added % perRow == 0)
                {
                    currentRow = CreateRow();
                    if (currentRow == null)
                        return;
                }

                var button = Instantiate(itemButtonTemplate, currentRow, false);
                button.gameObject.SetActive(true);
                button.SetupDrink(item, _selectionService);
                added++;
            }
        }
    }
}
