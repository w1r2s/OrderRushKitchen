using OrderRushKitchen.Level;
using System;
using Zenject;

namespace OrderRushKitchen.Game
{
    public class GameService : IGameService
    {
        private readonly ICurrentLevelProvider _level;
        public event EventHandler OnGameStateChanged;
        private enum GameState
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }
        private GameState state;
        private float waitingToStartTimer = 1f;
        private float countdownToStartTimer = 3f;
        private float gamePlayingTimer;
        private float gamePlayingTimerMax;

        [Inject]
        public GameService(ICurrentLevelProvider level)
        {
            state = GameState.WaitingToStart;
            _level = level;
        }
        public void Tick(float deltaTime)
        {
            switch (state)
            {
                case GameState.WaitingToStart:
                    waitingToStartTimer -= deltaTime;
                    if (waitingToStartTimer < 0f)
                    {
                        PrepareLevelTimer();
                        state = GameState.CountdownToStart;
                        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case GameState.CountdownToStart:
                    countdownToStartTimer -= deltaTime;
                    if (countdownToStartTimer < 0f)
                    {
                        state = GameState.GamePlaying;
                        gamePlayingTimer = gamePlayingTimerMax;
                        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case GameState.GamePlaying:
                    gamePlayingTimer -= deltaTime;
                    if (gamePlayingTimer < 0f)
                    {
                        state = GameState.GameOver;
                        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case GameState.GameOver:
                    break;
            }
        }
        public bool IsGamePlaying()
        {
            return state == GameState.GamePlaying;
        }
        public bool IsCountdownToStartActive()
        {
            return state == GameState.CountdownToStart;
        }
        public float GetCountdownToStartTimer()
        {
            return countdownToStartTimer;
        }
        public bool IsGameOver()
        {
            return state == GameState.GameOver;
        }
        public float GetGamePlayingTimerNormalized()
        {
            if (gamePlayingTimerMax <= 0f)
                return 0f;

            var normalized = 1f - gamePlayingTimer / gamePlayingTimerMax;

            return Math.Max(0f, Math.Min(1f, normalized));
        }
        public void RestartLevelRun()
        {
            waitingToStartTimer = 1f;
            countdownToStartTimer = 3f;
            gamePlayingTimer = 0f;
            gamePlayingTimerMax = 0f;
            state = GameState.WaitingToStart;

            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PrepareLevelTimer()
        {
            var level = _level.CurrentLevel;

            if (level == null)
                throw new InvalidOperationException("GameService: current level is not initialized.");

            gamePlayingTimerMax = Math.Max(1f, level.levelDurationSeconds);
            gamePlayingTimer = gamePlayingTimerMax;
        }
    }
}
