using OrderRushKitchen.Order;
using OrderRushKitchen.UserProgress;
using System;
using Zenject;

namespace OrderRushKitchen.Level
{
    public class LevelProgressionRuntime : IInitializable, IDisposable
    {
        private readonly ILevelProgressionService _levelProgressionService;
        private readonly IOrderService _orderService;
        private readonly IUserProgressService _userProgressService;
        private readonly ICurrentLevelProvider _currentLevelProvider;

        [Inject]
        public LevelProgressionRuntime(ILevelProgressionService levelProgressionService, IOrderService orderService, IUserProgressService userProgressService, ICurrentLevelProvider currentLevelProvider)
        {
            _levelProgressionService = levelProgressionService;
            _orderService = orderService;
            _userProgressService = userProgressService;
            _currentLevelProvider = currentLevelProvider;
        }

        public void Initialize()
        {
            _orderService.OnOrderCompleted += OrderService_OnOrderCompleted;
            _orderService.OnOrderFailed += OrderService_OnOrderFailed;
            _levelProgressionService.OnLevelCompleted += LevelProgressionService_OnLevelCompleted;
            _currentLevelProvider.OnCurrentLevelChanged += CurrentLevelProvider_OnCurrentLevelChanged;
        }


        public void Dispose()
        {
            if (_orderService != null)
            {
                _orderService.OnOrderCompleted -= OrderService_OnOrderCompleted;
                _orderService.OnOrderFailed -= OrderService_OnOrderFailed;
            }

            if (_levelProgressionService != null)
                _levelProgressionService.OnLevelCompleted -= LevelProgressionService_OnLevelCompleted;

            if (_currentLevelProvider != null)
                _currentLevelProvider.OnCurrentLevelChanged -= CurrentLevelProvider_OnCurrentLevelChanged;
        }

        private void OrderService_OnOrderCompleted(object sender, OrderServiceEventArgs e)
        {
            _levelProgressionService.RegisterOrderCompleted();
        }

        private void OrderService_OnOrderFailed(object sender, OrderServiceEventArgs e)
        {
            _levelProgressionService.RegisterOrderFailed();
        }

        private void LevelProgressionService_OnLevelCompleted(object sender, EventArgs e)
        {
            _userProgressService.CompleteLevel(_levelProgressionService.CurrentLevelIndex);
        }
        private void CurrentLevelProvider_OnCurrentLevelChanged(object sender, EventArgs e)
        {
            _userProgressService.SetCurrentLevel(_currentLevelProvider.CurrentLevel.levelNumber);
        }
    }
}
