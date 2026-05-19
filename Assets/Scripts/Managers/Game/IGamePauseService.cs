using System;

namespace Assets.Scripts.Managers.Game
{
    public interface IGamePauseService
    {
        bool IsPaused { get; }
        event EventHandler OnPauseChanged;
        void AddPause(GamePauseReason reason);
        void RemovePause(GamePauseReason reason);
        void ToggleUserPause();
        bool HasPause(GamePauseReason reason);
    }
}
