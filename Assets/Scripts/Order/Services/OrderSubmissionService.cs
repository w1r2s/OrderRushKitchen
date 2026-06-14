using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;

namespace OrderRushKitchen.Order
{
    public class OrderSubmissionService : IOrderSubmissionService
    {
        private readonly IOrderService _orderService;
        public OrderSubmissionService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public bool TrySubmit(MenuItemDefinitionSo menuItem, out OrderSubmissionFailureReason submissionResult)
        {
            submissionResult = default;
            if (menuItem == null)
            {
                submissionResult = OrderSubmissionFailureReason.NoItemsDelivered;
                return false;
            }
            var activeOrders = _orderService.GetActiveOrders();
            if (activeOrders.Count == 0)
            {
                submissionResult = OrderSubmissionFailureReason.NoActiveOrders;
                return false;
            }

            if (_orderService.TryFulfillOrderItem(menuItem))
                return true;

            submissionResult = OrderSubmissionFailureReason.NoMatchFound;
            return false;
        }
    }
}