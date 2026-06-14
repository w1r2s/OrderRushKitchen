using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;
using System;

namespace OrderRushKitchen.Order
{
    public interface IOrderService
    {
        void Tick(float deltaTime);
        ActiveOrder CreateOrder(IEnumerable<MenuItemDefinitionSo> items);
        IReadOnlyList<ActiveOrder> GetActiveOrders();
        bool TryFulfillOrderItem(MenuItemDefinitionSo menuItem);

        void ClearAllOrders();

        public event EventHandler<OrderServiceEventArgs> OnOrderCreated;
        public event EventHandler<OrderServiceEventArgs> OnOrderRemoved;
        public event EventHandler<OrderServiceEventArgs> OnOrderUpdated;
        public event EventHandler<OrderServiceEventArgs> OnOrderCompleted;
        public event EventHandler<OrderServiceEventArgs> OnOrderFailed;
    }
}