namespace OrderRushKitchen.Order
{
    public interface IOrderFlowService
    {
        OrderFlowResult TryCreateOrder();
    }
}