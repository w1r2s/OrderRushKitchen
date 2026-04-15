namespace Assets.Scripts.Order
{
    public interface IOrderCreationService
    {
        bool TryCreateOrder(out ActiveOrder order);
    }
}
