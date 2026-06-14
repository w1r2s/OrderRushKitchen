using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OrderRushKitchen.Order
{
    public class OrderService : IOrderService
    {
        public event EventHandler<OrderServiceEventArgs> OnOrderCreated;
        public event EventHandler<OrderServiceEventArgs> OnOrderRemoved;
        public event EventHandler<OrderServiceEventArgs> OnOrderUpdated;
        public event EventHandler<OrderServiceEventArgs> OnOrderCompleted;
        public event EventHandler<OrderServiceEventArgs> OnOrderFailed;

        private readonly List<ActiveOrder> _orders;

        private bool needsCleanup;
        private const float inactiveRemoveDelay = 1.25f;

        public OrderService()
        {
            _orders = new List<ActiveOrder>();
        }
        public ActiveOrder CreateOrder(IEnumerable<MenuItemDefinitionSo> items)
        {
            if (items == null)
                return null;

            var validItems = items.Where(item => item != null).ToList();

            if (validItems.Count == 0)
                return null;

            var order = new ActiveOrder(Guid.NewGuid().ToString(), validItems);

            _orders.Add(order);

            OnOrderCreated?.Invoke(this, new OrderServiceEventArgs(order));
            return order;
        }

        public IReadOnlyList<ActiveOrder> GetActiveOrders()
        {
            return _orders.Where(order => order.IsActive == true).ToList();
        }

        public void Tick(float deltaTime)
        {
            foreach (var order in _orders)
            {
                if (order.IsActive)
                {
                    order.Tick(deltaTime);

                    if (order.IsFailed)
                    {
                        OnOrderFailed?.Invoke(this, new OrderServiceEventArgs(order));
                    }
                }

                if (!order.IsActive)
                {
                    order.InactiveElapsed += deltaTime;

                    needsCleanup |= order.InactiveElapsed >= inactiveRemoveDelay;
                }
            }

            if (needsCleanup)
            {
                RemoveInactiveOrders();
                needsCleanup = false;
            }
        }
        public void RemoveInactiveOrders()
        {
            var removeCount = _orders.Where(order => !order.IsActive).Count();
            if (removeCount == 0)
                return;

            for (var i = _orders.Count - 1; i >= 0; i--)
            {
                if (_orders[i].IsActive)
                    continue;

                if (_orders[i].InactiveElapsed >= inactiveRemoveDelay)
                {
                    var order = _orders[i];
                    _orders.RemoveAt(i);
                    OnOrderRemoved?.Invoke(this, new OrderServiceEventArgs(order));
                }
            }
        }

        public bool TryFulfillOrderItem(MenuItemDefinitionSo menuItem)
        {
            if (menuItem == null)
                return false;

            foreach (var order in _orders)
            {
                if (!order.IsActive)
                    continue;

                if (order.TryFulfill(menuItem))
                {
                    if (order.IsCompleted)
                    {
                        OnOrderCompleted?.Invoke(this, new OrderServiceEventArgs(order));
                    }
                    else
                    {
                        OnOrderUpdated?.Invoke(this, new OrderServiceEventArgs(order));
                    }
                    return true;
                }
            }

            return false;
        }
        public void ClearAllOrders()
        {
            for (var i = _orders.Count - 1; i >= 0; i--)
            {

                var order = _orders[i];
                _orders.RemoveAt(i);
                OnOrderRemoved?.Invoke(this, new OrderServiceEventArgs(order));

            }

            needsCleanup = false;
        }
    }
}
