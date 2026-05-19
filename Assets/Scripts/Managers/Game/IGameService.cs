using System;

namespace Assets.Scripts.Managers.Game
{
    public interface IGameService
    {
        void Tick(float deltaTime);
        bool IsGamePlaying();
        bool IsCountdownToStartActive();
        float GetCountdownToStartTimer();
        bool IsGameOver();
        float GetGamePlayingTimerNormalized();
        void ResetGamePlayingTimer();

        event EventHandler OnGameStateChanged;
    }
}
