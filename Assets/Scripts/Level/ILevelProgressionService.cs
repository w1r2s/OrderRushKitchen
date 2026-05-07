using System;

namespace Assets.Scripts.Level
{
    public interface ILevelProgressionService
    {
        void RegisterOrderCompleted();
        bool TryAdvanceToNextLevel();
        int CurrentLevelIndex { get; }
        int CompletedOrdersInLevel { get; }
        int OrdersToCompleteForCurrentLevel { get; }
        bool IsLevelCompleted { get; }
        event EventHandler OnProgressChanged;
        event EventHandler OnLevelCompleted;
    }
}
