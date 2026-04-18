namespace Assets.Scripts.Order
{
    public interface IOrderFlowService
    {
        OrderFlowResult TryCreateOrder();
    }
}