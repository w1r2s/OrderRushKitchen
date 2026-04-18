namespace Assets.Scripts.Order
{
    public class OrderFlowResult
    {
        public bool Success { get; }
        public ActiveOrder Order { get; }
        public OrderFlowFailureReason? FailureReason { get; }

        public OrderFlowResult(bool success, ActiveOrder order, OrderFlowFailureReason? failureReason)
        {
            Success = success;
            Order = order;
            FailureReason = failureReason;
        }
    }
}
