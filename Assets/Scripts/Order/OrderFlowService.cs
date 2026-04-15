using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Order
{
    public class OrderFlowService : IOrderFlowService
    {

        private readonly IOrderService _orderService;
        private readonly IOrderGenerationService _orderGenerationService;
        private readonly LevelDefinitionSo _levelConfig;
        public OrderFlowService(IOrderService orderService, IOrderGenerationService orderGenerationService, LevelDefinitionSo levelConfig)
        {
            _orderService = orderService;
            _orderGenerationService = orderGenerationService;
            _levelConfig = levelConfig;
        }

        public OrderFlowResult TryCreateOrder()
        {
            if (_orderService.GetActiveOrders().Count >= _levelConfig.maxActiveOrders)
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