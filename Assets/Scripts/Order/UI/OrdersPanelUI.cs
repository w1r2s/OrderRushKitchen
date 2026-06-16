using System.Collections.Generic;
using System;
using UnityEngine;

namespace OrderRushKitchen.Order
{
    public class OrdersPanelUI : MonoBehaviour
    {
        private sealed class CardBinding
        {
            public ActiveOrder Order;
            public OrderCardUI Card;
        }

        public event Action<ActiveOrder> OrderSelected;

        [SerializeField] private Transform ordersArea;
        [SerializeField] private OrderCardUI orderCardTemplate;

        private readonly Dictionary<string, CardBinding> _orders = new();

        private void OnDestroy()
        {
            foreach (var binding in _orders.Values)
            {
                if (binding?.Card != null)
                    binding.Card.Clicked -= OrderCard_Clicked;
            }
        }

        public void Initialize()
        {
            if (orderCardTemplate != null)
                orderCardTemplate.gameObject.SetActive(false);
        }

        public void UpsertCard(ActiveOrder order)
        {
            if (order == null)
                return;

            if (_orders.TryGetValue(order.Id, out var binding) && binding?.Card != null)
            {
                binding.Order = order;
                binding.Card.Refresh(order);
                return;
            }

            if (ordersArea == null || orderCardTemplate == null)
                return;

            var newCard = Instantiate(orderCardTemplate, ordersArea, false);
            newCard.gameObject.SetActive(true);
            newCard.Bind(order);
            newCard.Clicked += OrderCard_Clicked;

            _orders[order.Id] = new CardBinding
            {
                Order = order,
                Card = newCard
            };
        }

        public void RemoveCard(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return;

            if (!_orders.TryGetValue(orderId, out var binding))
                return;

            if (binding.Card != null)
            {
                binding.Card.Clicked -= OrderCard_Clicked;
                Destroy(binding.Card.gameObject);
            }

            _orders.Remove(orderId);
        }

        public void RefreshCards()
        {
            if (_orders.Count == 0)
                return;

            foreach (var binding in _orders.Values)
            {
                if (binding?.Order == null || binding.Card == null)
                    continue;

                binding.Card.Refresh(binding.Order);
            }
        }

        public void Clear()
        {
            foreach (var binding in _orders.Values)
            {
                if (binding?.Card == null)
                    continue;

                binding.Card.Clicked -= OrderCard_Clicked;
                Destroy(binding.Card.gameObject);
            }

            _orders.Clear();
        }

        private void OrderCard_Clicked(ActiveOrder order)
        {
            if (order == null)
                return;

            OrderSelected?.Invoke(order);
        }
    }
}