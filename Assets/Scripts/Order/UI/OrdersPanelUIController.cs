using System;
using Zenject;

namespace OrderRushKitchen.Order
{
    public class OrdersPanelUIController : IInitializable, IDisposable, ITickable
    {
        private readonly IOrderService _orderService;
        private readonly OrdersPanelUI _ordersPanelUI;
        private readonly OrderDetailsUI _orderDetailsUI;

        public OrdersPanelUIController(IOrderService orderService, OrdersPanelUI ordersPanelUI, OrderDetailsUI orderDetailsUI)
        {
            _orderService = orderService;
            _ordersPanelUI = ordersPanelUI;
            _orderDetailsUI = orderDetailsUI;
        }

        public void Initialize()
        {
            _ordersPanelUI.Initialize();
            _ordersPanelUI.OrderSelected += OrdersPanelUI_OrderSelected;

            _orderService.OnOrderCreated += OrderService_OnOrderCreated;
            _orderService.OnOrderUpdated += OrderService_OnOrderChanged;
            _orderService.OnOrderFailed += OrderService_OnOrderChanged;
            _orderService.OnOrderCompleted += OrderService_OnOrderChanged;
            _orderService.OnOrderRemoved += OrderService_OnOrderRemoved;

            InitializeExistingOrders();
        }

        public void Dispose()
        {
            _ordersPanelUI.OrderSelected -= OrdersPanelUI_OrderSelected;

            _orderService.OnOrderCreated -= OrderService_OnOrderCreated;
            _orderService.OnOrderUpdated -= OrderService_OnOrderChanged;
            _orderService.OnOrderFailed -= OrderService_OnOrderChanged;
            _orderService.OnOrderCompleted -= OrderService_OnOrderChanged;
            _orderService.OnOrderRemoved -= OrderService_OnOrderRemoved;
        }

        public void Tick()
        {
            _ordersPanelUI.RefreshCards();
        }

        private void InitializeExistingOrders()
        {
            var activeOrders = _orderService.GetActiveOrders();
            for (int i = 0; i < activeOrders.Count; i++)
            {
                _ordersPanelUI.UpsertCard(activeOrders[i]);
            }
        }

        private void OrderService_OnOrderCreated(object sender, OrderServiceEventArgs e)
        {
            if (e?.Order == null)
                return;

            _ordersPanelUI.UpsertCard(e.Order);
        }

        private void OrderService_OnOrderChanged(object sender, OrderServiceEventArgs e)
        {
            if (e?.Order == null)
                return;

            _ordersPanelUI.UpsertCard(e.Order);
        }

        private void OrderService_OnOrderRemoved(object sender, OrderServiceEventArgs e)
        {
            if (e?.Order == null)
                return;

            _ordersPanelUI.RemoveCard(e.Order.Id);
        }

        private void OrdersPanelUI_OrderSelected(ActiveOrder order)
        {
            if (order == null)
                return;

            _orderDetailsUI.Show(order);
        }
    }
}