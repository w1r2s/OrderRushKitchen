using Assets.Scripts.Order;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class OrderCounter : BaseCounter
    {
        [SerializeField] private List<MenuItemDefinitionSo> orderItems;

        private IOrderService _orderService;

        [Inject]
        private void Construct(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public override void Interact(Player player)
        {
          var order = _orderService.CreateOrder(orderItems);
            if (order == null)
            {
                Debug.Log("Failed to create order: no valid menu items configured.");
                return;
            }
            Debug.Log($"Order created:\n" +
                $"order id: {order.Id},\n" +
                $"order items count: {order.OrderItems.Count},\n" +
                $"order max time: {order.MaxTime}");
        }
    }
}
