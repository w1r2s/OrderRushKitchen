using OrderRushKitchen.Menu;
using System;
using System.Collections.Generic;

namespace OrderRushKitchen.Order
{
    public interface IOrderService
    {
        void Tick(float deltaTime);
        ActiveOrder CreateOrder(IEnumerable<MenuItemDefinitionSo> items);
        IReadOnlyList<ActiveOrder> GetActiveOrders();
        bool TryFulfillOrderItem(MenuItemDefinitionSo menuItem, out ActiveOrder fulfilledOrder, out OrderItem fulfilledItem);
        bool TryFulfillOrderItemForOrder(ActiveOrder targetOrder, MenuItemDefinitionSo menuItem, out OrderItem fulfilledItem);
        bool TryRevokeFulfilledItems(ActiveOrder order, IReadOnlyCollection<OrderItem> items);
        void ClearAllOrders();

        public event EventHandler<OrderServiceEventArgs> OnOrderCreated;
        public event EventHandler<OrderServiceEventArgs> OnOrderRemoved;
        public event EventHandler<OrderServiceEventArgs> OnOrderUpdated;
        public event EventHandler<OrderServiceEventArgs> OnOrderCompleted;
        public event EventHandler<OrderServiceEventArgs> OnOrderFailed;
    }
}
