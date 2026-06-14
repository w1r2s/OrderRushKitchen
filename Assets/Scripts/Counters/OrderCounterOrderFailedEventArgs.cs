using OrderRushKitchen.Order;
using System;

namespace OrderRushKitchen.Counters
{
    public class OrderCounterOrderFailedEventArgs : EventArgs
    {
        public OrderFlowFailureReason FailureReason { get; }

        public OrderCounterOrderFailedEventArgs(OrderFlowFailureReason reason)
        {
            FailureReason = reason;
        }
    }
}
