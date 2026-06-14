namespace OrderRushKitchen.Order
{
    public enum OrderSubmissionFailureReason
    {
        None = 0,
        NoItemsDelivered = 1,
        NoActiveOrders = 2,
        NoMatchFound = 3,
    }
}