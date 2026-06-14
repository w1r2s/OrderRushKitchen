using OrderRushKitchen.Order;

namespace OrderRushKitchen.Counters
{
    public sealed class OrderStagingReservation
    {
        public ActiveOrder Order { get; }

        internal OrderStagingReservation(ActiveOrder order)
        {
            Order = order;
        }
    }
}
