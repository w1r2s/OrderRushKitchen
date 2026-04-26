using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Order
{
    public class OrderCardUI : MonoBehaviour
    {
        [SerializeField] private Slider progressSlider;
        [SerializeField] private Transform itemsSectionsContainer;
        [SerializeField] private OrderItemSectionUI orderItemSectionTemplate;

        private readonly List<OrderItemSectionUI> _sections = new();

        public void Bind(ActiveOrder order)
        {
            if (orderItemSectionTemplate != null)
            {
                orderItemSectionTemplate.gameObject.SetActive(false);
            }

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
