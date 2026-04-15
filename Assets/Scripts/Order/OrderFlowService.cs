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

        public bool TryCreateOrder(out ActiveOrder order)
        {
            order = null;

            if (_orderService.GetActiveOrders().Count >= _levelConfig.maxActiveOrders)
                return false;

            var menuItemList = _orderGenerationService.GenerateOrderItems();
            if (menuItemList == null)
            {
                return false;
            }

            order = _orderService.CreateOrder(menuItemList);
            if (order == null)
            {
                return false;
            }


            return true;
        }

    }
}