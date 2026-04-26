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


        [SerializeField] private GameObject selectionPanel;
        [SerializeField] private GameObject dimmer;

        [SerializeField] private Transform shelvesArea;
        [SerializeField] private Button closeButton;
        [SerializeField] private RectTransform ShelfRowTemplate;
        [SerializeField] private ItemSelectionButtonUI ItemButtonTemplate;

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

            ShelfRowTemplate.gameObject.SetActive(false);
            Hide();
        }
        private void OnDestroy()
        {
            _selectionService.OnSelectionOpened -= SelectionService_OnSelectionOpened;
            _selectionService.OnSelectionClosed -= SelectionService_OnSelectionClosed;

            closeButton.onClick.RemoveListener(Cancel);
        }
        private void SelectionService_OnSelectionOpened(object sender, EventArgs e)
        {
            ClearRows();

            if (_selectionService.CurrentMode == ItemSelectionMode.Ingredients)
            {
                BuildIngredientRows();
            }
            if (_selectionService.CurrentMode == ItemSelectionMode.Drinks)
            {
                BuildMenuItemRows();
            }

            Show();
        }
        private void SelectionService_OnSelectionClosed(object sender, EventArgs e)
        {
            ClearRows();
            Hide();
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
            var row = Instantiate(ShelfRowTemplate, shelvesArea, false);
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
            RectTransform currentRow = null;

            var perRow = Mathf.Max(1, itemsPerRow);

            var options = _selectionService.CurrentIngredientOptions;

            if (options == null || options.Count == 0)
                return;

            int added = 0;
            for (int i = 0; i < _selectionService.CurrentIngredientOptions.Count; i++)
            {
                var item = options[i];
                if (item == null)
                    continue;
                if (added % perRow == 0)
                {
                    currentRow = CreateRow();
                }
                var button = Instantiate(ItemButtonTemplate, currentRow, false);
                button.gameObject.SetActive(true);
                button.SetupIngredient(item, _selectionService);
                added++;
            }
        }
        private void BuildMenuItemRows()
        {
            RectTransform currentRow = null;

            var perRow = Mathf.Max(1, itemsPerRow);

            var options = _selectionService.CurrentDrinkOptions;

            if (options == null || options.Count == 0)
                return;

            int added = 0;
            for (int i = 0; i < options.Count; i++)
            {
                var item = options[i];
                if (item == null || item.category != MenuItemCategory.Drink)
                    continue;

                if (added % perRow == 0)
                {
                    currentRow = CreateRow();
                }
                var button = Instantiate(ItemButtonTemplate, currentRow, false);
                button.gameObject.SetActive(true);
                button.SetupDrink(item, _selectionService);
                added++;
            }
        }
    }
}
