using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Order
{
    public class OrderService : IOrderService
    {
        public event EventHandler<OrderServiceEventArgs> OnOrderCreated;
        public event EventHandler<OrderServiceEventArgs> OnOrderRemoved;
        public event EventHandler<OrderServiceEventArgs> OnOrderUpdated;
        public event EventHandler<OrderServiceEventArgs> OnOrderCompleted;
        public event EventHandler<OrderServiceEventArgs> OnOrderFailed;

        private readonly List<ActiveOrder> _orders;

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
                if (!order.IsActive) continue;

                order.Tick(deltaTime);

                if (order.IsFailed)
                {
                    OnOrderFailed?.Invoke(this, new OrderServiceEventArgs(order));
                    continue;
                }
            }
        }
        public int RemoveInactiveOrders()
        {
            var removeCount = _orders.Where(order => !order.IsActive).Count();
            if (removeCount == 0)
                return 0;

            for (var i = _orders.Count - 1; i >= 0; i--)
            {
                if (_orders[i].IsActive)
                    continue;
                var order = _orders[i];
                _orders.RemoveAt(i);
                OnOrderRemoved?.Invoke(this, new OrderServiceEventArgs(order));
            }

            return removeCount;
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
                        OnOrderCompleted?.Invoke(this, new OrderServiceEventArgs(order));
                    else
                        OnOrderUpdated?.Invoke(this, new OrderServiceEventArgs(order));

                    return true;
                }
            }

            return false;
        }
    }
}
