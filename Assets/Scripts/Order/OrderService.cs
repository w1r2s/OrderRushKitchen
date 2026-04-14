using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Order
{
    public class OrderService : IOrderService
    {
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

            if( validItems.Count == 0 )
                return null;

            var order = new ActiveOrder(Guid.NewGuid().ToString(), validItems);

            _orders.Add(order);

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
                if(!order.IsActive) continue;

                order.Tick(deltaTime);

            }
        }
        public void RemoveInactiveOrders()
        {
            _orders.RemoveAll(order => !order.IsActive);
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
                    return true;
            }

            return false;
        }
    }
}
