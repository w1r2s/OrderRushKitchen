using Assets.Scripts.Order;
using System;

namespace Assets.Scripts.Counters
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
