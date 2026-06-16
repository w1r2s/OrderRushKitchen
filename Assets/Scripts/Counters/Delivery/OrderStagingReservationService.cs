using OrderRushKitchen.Menu;
using OrderRushKitchen.Order;
using System.Collections.Generic;

namespace OrderRushKitchen.Counters
{
    public sealed class OrderStagingReservationService : IOrderStagingReservationService
    {
        private readonly IOrderService _orderService;
        private readonly Dictionary<ActiveOrder, OrderStagingReservation> _reservations = new();

        public OrderStagingReservationService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public bool TryReserveFor(MenuItemDefinitionSo menuItem, out OrderStagingReservation reservation)
        {
            reservation = null;

            if (menuItem == null)
                return false;

            IReadOnlyList<ActiveOrder> activeOrders = _orderService.GetActiveOrders();

            for (int i = 0; i < activeOrders.Count; i++)
            {
                ActiveOrder order = activeOrders[i];

                if (order == null || _reservations.ContainsKey(order) || !order.CanFulfill(menuItem))
                {
                    continue;
                }

                reservation = new OrderStagingReservation(order);
                _reservations.Add(order, reservation);
                return true;
            }

            return false;
        }

        public void Release(OrderStagingReservation reservation)
        {
            if (reservation?.Order == null)
                return;

            if (_reservations.TryGetValue(reservation.Order, out OrderStagingReservation current) && ReferenceEquals(current, reservation))
            {
                _reservations.Remove(reservation.Order);
            }
        }
    }
}
