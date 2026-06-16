using System;

namespace OrderRushKitchen.Order
{
    public class OrderServiceEventArgs : EventArgs
    {
        public ActiveOrder Order { get; }
        public OrderServiceEventArgs(ActiveOrder order)
        {
            Order = order;
        }
    }
}