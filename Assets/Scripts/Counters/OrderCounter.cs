using Assets.Scripts.Order;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class OrderCounter : BaseCounter
    {
        private IOrderFlowService _orderFlowService;

        [Inject]
        private void Construct(IOrderFlowService orderFlowService)
        {
            _orderFlowService = orderFlowService;
        }
        public override void Interact(Player player)
        {
            if (!_orderFlowService.TryCreateOrder(out var order))
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
