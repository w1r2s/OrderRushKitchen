using Assets.Scripts.Order;
using System;
using Zenject;

namespace Assets.Scripts.Level
{
    public class LevelProgressionRuntime : IInitializable, IDisposable
    {
        private ILevelProgressionService _levelProgressionService;
        private IOrderService _orderService;

        [Inject]
        public LevelProgressionRuntime(ILevelProgressionService levelProgressionService, IOrderService orderService)
        {
            _levelProgressionService = levelProgressionService;
            _orderService = orderService;
        }
        private void OrderService_OnOrderCompleted(object sender, OrderServiceEventArgs e)
        {
            _levelProgressionService.RegisterOrderCompleted();
        }

        public void Initialize()
        {
            _orderService.OnOrderCompleted += OrderService_OnOrderCompleted;
        }

        public void Dispose()
        {
            if (_orderService != null)
                _orderService.OnOrderCompleted -= OrderService_OnOrderCompleted;

        }
    }
}
