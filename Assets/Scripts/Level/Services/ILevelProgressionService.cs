using System;

namespace OrderRushKitchen.Level
{
    public interface ILevelProgressionService
    {
        void RegisterOrderCompleted();
        bool TryAdvanceToNextLevel();
        void RegisterOrderFailed();
        int FailedOrdersInLevel { get; }
        int CurrentLevelIndex { get; }
        int CompletedOrdersInLevel { get; }
        int OrdersToCompleteForCurrentLevel { get; }
        bool IsLevelCompleted { get; }
        event EventHandler OnProgressChanged;
        event EventHandler OnLevelCompleted;
    }
}
