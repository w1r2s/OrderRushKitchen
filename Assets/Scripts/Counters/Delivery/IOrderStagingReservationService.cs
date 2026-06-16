using OrderRushKitchen.Menu;

namespace OrderRushKitchen.Counters
{
    public interface IOrderStagingReservationService
    {
        bool TryReserveFor(MenuItemDefinitionSo menuItem, out OrderStagingReservation reservation);

        void Release(OrderStagingReservation reservation);
    }
}
