using System;

namespace OrderRushKitchen.Level
{
    public class LevelProgressionService : ILevelProgressionService
    {
        public event EventHandler OnProgressChanged;
        public event EventHandler OnLevelCompleted;

        private readonly ICurrentLevelProvider _currentLevelProvider;
        public int CurrentLevelIndex => _currentLevelProvider.CurrentLevel.levelNumber;
        public int CompletedOrdersInLevel { get; private set; }
        public int OrdersToCompleteForCurrentLevel => _currentLevelProvider.CurrentLevel.requiredCompletedOrders <= 0 ? 1 : _currentLevelProvider.CurrentLevel.requiredCompletedOrders;
        public bool IsLevelCompleted { get; private set; }

        public LevelProgressionService(ICurrentLevelProvider levelProvider)
        {
            _currentLevelProvider = levelProvider;
        }

        public void RegisterOrderCompleted()
        {
            if (IsLevelCompleted)
                return;

            CompletedOrdersInLevel++;
            OnProgressChanged?.Invoke(this, EventArgs.Empty);

            if (CompletedOrdersInLevel >= OrdersToCompleteForCurrentLevel)
            {
                IsLevelCompleted = true;
                OnLevelCompleted?.Invoke(this, EventArgs.Empty);
            }
        }
        public bool TryAdvanceToNextLevel()
        {
            if (!IsLevelCompleted)
                return false;
            if (!_currentLevelProvider.TryMoveToNextLevel())
                return false;

            CompletedOrdersInLevel = 0;
            IsLevelCompleted = false;
            OnProgressChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }
    }
}
