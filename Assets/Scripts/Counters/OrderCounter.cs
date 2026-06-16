using OrderRushKitchen.Order;
using OrderRushKitchen.PlayerControl;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Counters
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
                return;
            }
            OnOrderAccepted?.Invoke(this, EventArgs.Empty);
        }
    }
}
