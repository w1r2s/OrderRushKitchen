using Assets.Scripts.Order;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class OrderCounter : BaseCounter
    {
        public event EventHandler OnOrderAccepted;
        public event EventHandler<OrderCounterOrderFailedEventArgs> OnOrderAcceptFailed;

        private IOrderFlowService _orderFlowService;

        [Inject]
        private void Construct(IOrderFlowService orderFlowService)
        {
            _orderFlowService = orderFlowService;
        }
        public override void Interact(Player player)
        {
            var orderResult = _orderFlowService.TryCreateOrder();
            if (!orderResult.Success)
            {
                if (orderResult.FailureReason.HasValue)
                {
                    OnOrderAcceptFailed?.Invoke(this, new OrderCounterOrderFailedEventArgs(orderResult.FailureReason.Value));
                }

                Debug.Log($"Failed to create order: {orderResult.FailureReason}");
                return;
            }
            OnOrderAccepted?.Invoke(this, EventArgs.Empty);


            Debug.Log($"Order created:\n" +
                $"order id: {orderResult.Order.Id},\n" +
                $"order items count: {orderResult.Order.OrderItems.Count},\n" +
                $"order max time: {orderResult.Order.MaxTime}");
        }
    }
}
