using System;

namespace OrderRushKitchen.Order
{
    public sealed class OrderSubmissionResult
    {
        public bool Success { get; }
        public ActiveOrder Order { get; }
        public OrderItem FulfilledItem { get; }
        public OrderSubmissionFailureReason FailureReason { get; }

        private OrderSubmissionResult(bool success, ActiveOrder order, OrderItem fulfilledItem, OrderSubmissionFailureReason failureReason)
        {
            Success = success;
            Order = order;
            FulfilledItem = fulfilledItem;
            FailureReason = failureReason;
        }

        public static OrderSubmissionResult Succeeded(ActiveOrder order, OrderItem fulfilledItem)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (fulfilledItem == null)
                throw new ArgumentNullException(nameof(fulfilledItem));

            return new OrderSubmissionResult(true, order, fulfilledItem, OrderSubmissionFailureReason.None);
        }

        public static OrderSubmissionResult Failed(OrderSubmissionFailureReason failureReason)
        {
            if (failureReason == OrderSubmissionFailureReason.None)
            {
                throw new ArgumentOutOfRangeException(nameof(failureReason), failureReason, "Failure result must contain a failure reason.");
            }

            return new OrderSubmissionResult(false, null, null, failureReason);
        }
    }
}
