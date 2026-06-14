using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace OrderRushKitchen.Order
{
    public class OrderDetailsUI : MonoBehaviour
    {
        public event EventHandler Opened;
        public event EventHandler Closed;

        [SerializeField] private GameObject dimmer;
        [SerializeField] private GameObject detailsPanel;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button dimmerButton;

        [SerializeField] private Transform orderRowsSection;
        [SerializeField] private OrderRecipeRowUI recipeRowTemplate;

        private readonly List<OrderRecipeRowUI> _orderRows = new();

        private void Start()
        {
            if (recipeRowTemplate != null)
            {
                recipeRowTemplate.gameObject.SetActive(false);
            }
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Hide);
            }
            if (dimmerButton != null)
            {
                dimmerButton.onClick.AddListener(Hide);
            }

            Hide();
        }
        private void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Hide);
            }
            if (dimmerButton != null)
            {
                dimmerButton.onClick.RemoveListener(Hide);
            }
        }
        public void Hide()
        {
            var isOpen = (dimmer != null && dimmer.activeSelf) || (detailsPanel != null && detailsPanel.activeSelf);

            if (!isOpen)
                return;

            if (detailsPanel != null)
                detailsPanel.SetActive(false);

            if (dimmer != null)
                dimmer.SetActive(false);

            ClearRows();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public void Show(ActiveOrder order)
        {
            if (order == null)
            {
                Hide();
                return;
            }
            if (orderRowsSection == null || recipeRowTemplate == null)
            {
                Hide();
                return;
            }
            ClearRows();

            for (int i = 0; i < order.OrderItems.Count; i++)
            {
                var item = order.OrderItems[i];
                if (item == null)
                    continue;

                var row = Instantiate(recipeRowTemplate, orderRowsSection, false);
                row.gameObject.SetActive(true);
                row.Bind(item);
                _orderRows.Add(row);
            }

            if (_orderRows.Count == 0)
            {
                Hide();
                return;
            }

            if (detailsPanel != null && dimmer != null)
            {
                var wasOpen = dimmer.activeSelf || detailsPanel.activeSelf;

                detailsPanel.SetActive(true);
                dimmer.SetActive(true);

                if (!wasOpen)
                    Opened?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ClearRows()
        {
            for (int i = 0; i < _orderRows.Count; i++)
            {
                if (_orderRows[i] != null)
                    Destroy(_orderRows[i].gameObject);
            }

            _orderRows.Clear();
        }

    }
}