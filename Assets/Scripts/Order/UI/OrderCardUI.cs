using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Order
{
    public class OrderCardUI : MonoBehaviour
    {
        public event Action<ActiveOrder> Clicked; 

        [SerializeField] private Slider progressSlider;
        [SerializeField] private Transform itemsSectionsContainer;
        [SerializeField] private OrderItemSectionUI orderItemSectionTemplate;
        [SerializeField] private CanvasGroup cardCanvasGroup;
        [SerializeField] private float finalAlpha = 0.6f;
        [SerializeField] private Button cardButton;
        [SerializeField] private Image cardImage;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color completedColor;
        [SerializeField] private Color failedColor;
      
        private readonly List<OrderItemSectionUI> _sections = new();

        private ActiveOrder _order;

        private void OnEnable()
        {
            if (cardButton != null)
                cardButton.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            if (cardButton != null)
                cardButton.onClick.RemoveListener(Click);
        }

        private void Click()
        {
            if (_order == null)
                return;

            Clicked?.Invoke(_order);
        }
        public void Bind(ActiveOrder order)
        {
            if (orderItemSectionTemplate != null)
            {
                orderItemSectionTemplate.gameObject.SetActive(false);
            }
            _order = order;

            RebuildSections(order);
            Refresh(order);
        }

        public void Refresh(ActiveOrder order)
        {
            if (order == null)
                return;

            if (progressSlider != null)
            {
                progressSlider.value = order.ProgressNormalized;
            }

            if (cardCanvasGroup != null)
            {
                var isFinal = order.IsCompleted || order.IsFailed;
                cardCanvasGroup.alpha = isFinal ? finalAlpha : 1f;

            }
            if (cardImage != null)
            {
                if (order.IsFailed)
                    cardImage.color = failedColor;

                else if (order.IsCompleted)
                    cardImage.color = completedColor;

                else
                    cardImage.color = normalColor;
            }

            var count = Mathf.Min(order.OrderItems.Count, _sections.Count);
            for (int i = 0; i < count; i++)
            {
                _sections[i].Refresh(order.OrderItems[i]);
            }
        }

        private void RebuildSections(ActiveOrder order)
        {
            ClearSections();

            if (order == null || itemsSectionsContainer == null || orderItemSectionTemplate == null)
                return;

            for (int i = 0; i < order.OrderItems.Count; i++)
            {
                var orderItem = order.OrderItems[i];
                if (orderItem == null)
                    continue;

                var section = Instantiate(orderItemSectionTemplate, itemsSectionsContainer, false);
                section.gameObject.SetActive(true);
                section.Bind(orderItem);
                _sections.Add(section);
            }
        }

        private void ClearSections()
        {
            for (int i = 0; i < _sections.Count; i++)
            {
                if (_sections[i] != null)
                    Destroy(_sections[i].gameObject);
            }

            _sections.Clear();
        }
    }
}
