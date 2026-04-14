using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Order
{
    public interface IOrderService
    {
        void Tick(float deltaTime);
        ActiveOrder CreateOrder(IEnumerable<MenuItemDefinitionSo> items);
        IReadOnlyList<ActiveOrder> GetActiveOrders();
        void RemoveInactiveOrders();
        bool TryFulfillOrderItem(MenuItemDefinitionSo menuItem);
    }
}
