using System;

namespace Assets.Scripts.Level
{
    public sealed class LevelCompletionShownEventArgs : EventArgs
    {
        public int LevelIndex { get; }
        public int CompletedOrders { get; }
        public int RequiredOrders { get; }

        public LevelCompletionShownEventArgs(int levelIndex, int completedOrders, int requiredOrders)
        {
            LevelIndex = levelIndex;
            CompletedOrders = completedOrders;
            RequiredOrders = requiredOrders;
        }

    }

}
