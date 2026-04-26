using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Order
{
    public class OrdersPanelUI : MonoBehaviour
    {
        private sealed class CardBinding
        {
            public ActiveOrder Order;
            public OrderCardUI Card;
        }

        private IOrderService _orderService;
        private readonly Dictionary<string, CardBinding> _orders = new();

        [SerializeField] private GameObject ordersPanel;
        [SerializeField] private Transform ordersArea;
        [SerializeField] private OrderCardUI orderCardTemplate;

        [Inject]
        private void Construct(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private void Start()
        {
            if (orderCardTemplate != null)
            {
                orderCardTemplate.gameObject.SetActive(false);
            }

            _orderService.OnOrderCreated += OrderService_OnOrderCreated;
            _orderService.OnOrderUpdated += OrderService_Refresh;
            _orderService.OnOrderFailed += OrderService_Refresh;
            _orderService.OnOrderRemoved += OrderService_OnOrderRemoved;
            _orderService.OnOrderCompleted += OrderService_Refresh;

            Initialize();
        }

        private void OnDestroy()
        {
            if (_orderService == null)
                return;

            _orderService.OnOrderCreated -= OrderService_OnOrderCreated;
            _orderService.OnOrderUpdated -= OrderService_Refresh;
            _orderService.OnOrderFailed -= OrderService_Refresh;
            _orderService.OnOrderRemoved -= OrderService_OnOrderRemoved;
            _orderService.OnOrderCompleted -= OrderService_Refresh;
        }

        private void Update()
        {
            if (_orders.Count == 0)
                return;

            foreach (var pair in _orders)
            {
                var binding = pair.Value;
                if (binding?.Order == null || binding.Card == null)
                    continue;

                binding.Card.Refresh(binding.Order);
            }
        }

        private void Initialize()
        {
            var activeOrders = _orderService.GetActiveOrders();
            for (int i = 0; i < activeOrders.Count; i++)
            {
                UpsertCard(activeOrders[i]);
            }
        }

        private void OrderService_OnOrderCreated(object sender, OrderServiceEventArgs e)
        {
            if (e?.Order == null)
                return;

            UpsertCard(e.Order);
        }

        private void OrderService_OnOrderRemoved(object sender, OrderServiceEventArgs e)
        {
            if (e?.Order == null)
                return;

            if (!_orders.TryGetValue(e.Order.Id, out var binding) || binding.Card == null)
                return;

            Destroy(binding.Card.gameObject);
            _orders.Remove(e.Order.Id);
        }

        private void OrderService_Refresh(object sender, OrderServiceEventArgs e)
        {
            if (e?.Order == null)
                return;

            UpsertCard(e.Order);
        }

        private void UpsertCard(ActiveOrder order)
        {
            if (order == null)
                return;

            if (_orders.TryGetValue(order.Id, out var binding) && binding?.Card != null)
            {
                binding.Order = order;
                binding.Card.Refresh(order);
                return;
            }

            var newCard = Instantiate(orderCardTemplate, ordersArea, false);
            newCard.gameObject.SetActive(true);
            newCard.Bind(order);

            _orders[order.Id] = new CardBinding
            {
                Order = order,
                Card = newCard
            };
        }
    }
}
