using System;

namespace Assets.Scripts.Level
{
    public interface ILevelCompletionFlowService
    {
        bool IsCompletionOpen { get; }

        event EventHandler<LevelCompletionShownEventArgs> OnCompletionShown;
        event EventHandler OnCompletionHidden;
        event EventHandler OnReturnToMenuRequested;

        void ConfirmNextLevel();
        void RequestReturnToMenu();
    }

}
