using System;

namespace OrderRushKitchen.Game
{
    public interface IGameService
    {
        void Tick(float deltaTime);
        bool IsGamePlaying();
        bool IsCountdownToStartActive();
        float GetCountdownToStartTimer();
        bool IsGameOver();
        float GetGamePlayingTimerNormalized();
        void RestartLevelRun();

        event EventHandler OnGameStateChanged;
    }
}
