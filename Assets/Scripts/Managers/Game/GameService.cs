using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Game
{
    public class GameService : IGameService
    {
        public event EventHandler OnGameStateChanged;
        public event EventHandler OnGamePaused;
        public event EventHandler OnGameUnpaused;
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
        private float gamePlayingTimerMax = 300f;

        private bool isPaused = false;

        public GameService()
        {
            state = GameState.WaitingToStart;
        }
        public void Tick(float deltaTime)
        {
            switch (state)
            {
                case GameState.WaitingToStart:
                    waitingToStartTimer -= deltaTime;
                    if (waitingToStartTimer < 0f)
                    {
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
            return 1 - gamePlayingTimer / gamePlayingTimerMax;
        }
        public void TogglePauseGame()
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                isPaused = false;
                OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Time.timeScale = 0f;
                isPaused = true;
                OnGamePaused?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
