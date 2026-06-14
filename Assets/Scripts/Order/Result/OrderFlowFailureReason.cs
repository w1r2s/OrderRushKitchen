namespace OrderRushKitchen.Order
{
    public enum OrderFlowFailureReason
    {
        MaxActiveOrdersReached = 0,
        NoAvailableMenuItems = 1,
        InvalidGeneratedOrder = 2,
        OrderCreationFailed = 3
    }
}
