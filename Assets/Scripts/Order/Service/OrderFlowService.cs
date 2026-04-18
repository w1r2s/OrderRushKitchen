using Assets.Scripts.Level;

namespace Assets.Scripts.Order
{
    public class OrderFlowService : IOrderFlowService
    {

        private readonly IOrderService _orderService;
        private readonly IOrderGenerationService _orderGenerationService;
        private readonly ICurrentLevelProvider _currentLevel;
        public OrderFlowService(IOrderService orderService, IOrderGenerationService orderGenerationService, ICurrentLevelProvider levelConfig)
        {
            _orderService = orderService;
            _orderGenerationService = orderGenerationService;
            _currentLevel = levelConfig;
        }

        public OrderFlowResult TryCreateOrder()
        {
            if (_orderService.GetActiveOrders().Count >= _currentLevel.CurrentLevel.maxActiveOrders)
                return new OrderFlowResult(false, null, OrderFlowFailureReason.MaxActiveOrdersReached);

            var menuItemList = _orderGenerationService.GenerateOrderItems();
            if (menuItemList == null)
            {
                return new OrderFlowResult(false, null, OrderFlowFailureReason.InvalidGeneratedOrder);
            }
            var order = _orderService.CreateOrder(menuItemList);
            if (order == null)
            {
                return new OrderFlowResult(false, order, OrderFlowFailureReason.OrderCreationFailed);
            }

            return new OrderFlowResult(true, order, null);
        }

    }
}