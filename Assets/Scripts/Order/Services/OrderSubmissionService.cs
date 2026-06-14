using OrderRushKitchen.Menu;
using System.Linq;

namespace OrderRushKitchen.Order
{
    public class OrderSubmissionService : IOrderSubmissionService
    {
        private readonly IOrderService _orderService;
        public OrderSubmissionService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public OrderSubmissionResult TrySubmit(MenuItemDefinitionSo menuItem)
        {
            if (menuItem == null)
            {
                return OrderSubmissionResult.Failed(OrderSubmissionFailureReason.NoItemsDelivered);
            }

            if (_orderService.GetActiveOrders().Count == 0)
            {
                return OrderSubmissionResult.Failed(OrderSubmissionFailureReason.NoActiveOrders);
            }

            if (!_orderService.TryFulfillOrderItem(menuItem, out ActiveOrder fulfilledOrder, out OrderItem fulfilledItem))
            {
                return OrderSubmissionResult.Failed(OrderSubmissionFailureReason.NoMatchFound);
            }

            return OrderSubmissionResult.Succeeded(fulfilledOrder, fulfilledItem);
        }

        public OrderSubmissionResult TrySubmit(MenuItemDefinitionSo menuItem, ActiveOrder targetOrder)
        {
            if (menuItem == null)
            {
                return OrderSubmissionResult.Failed(OrderSubmissionFailureReason.NoItemsDelivered);
            }

            if (targetOrder == null || !_orderService.GetActiveOrders().Contains(targetOrder))
            {
                return OrderSubmissionResult.Failed(OrderSubmissionFailureReason.TargetOrderUnavailable);
            }

            if (!_orderService.TryFulfillOrderItemForOrder(targetOrder, menuItem, out OrderItem fulfilledItem))
            {
                return OrderSubmissionResult.Failed(OrderSubmissionFailureReason.NoMatchFound);
            }

            return OrderSubmissionResult.Succeeded(targetOrder, fulfilledItem);
        }
    }
}
