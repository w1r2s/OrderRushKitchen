namespace Assets.Scripts.Order
{
    public interface IOrderFlowService
    {
        bool TryCreateOrder(out ActiveOrder order);
    }
}
