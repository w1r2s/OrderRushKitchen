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
            var orderResult = _orderFlowService.TryCreateOrder();
            if(!orderResult.Success)
            {
                Debug.Log($"Failed to create order: {orderResult.FailureReason}");
                return;
            }
            Debug.Log($"Order created:\n" +
                $"order id: {orderResult.Order.Id},\n" +
                $"order items count: {orderResult.Order.OrderItems.Count},\n" +
                $"order max time: {orderResult.Order.MaxTime}");
        }
    }
}
