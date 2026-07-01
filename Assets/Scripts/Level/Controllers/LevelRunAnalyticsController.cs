using OrderRushKitchen.Game;
using OrderRushKitchen.Sdk;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Level
{
    public sealed class LevelRunAnalyticsController : IInitializable, IDisposable, ILevelRunAnalyticsService
    {
        private readonly IGameService _gameService;
        private readonly ILevelProgressionService _levelProgressionService;
        private readonly ILevelCompletionFlowService _levelCompletionFlowService;
        private readonly IAnalyticsService _analyticsService;

        private bool _isRunActive;
        private bool _hasLoggedGameOver;
        private float _runStartedAt;

        public LevelRunAnalyticsController(IGameService gameService,
            ILevelProgressionService levelProgressionService,
            ILevelCompletionFlowService levelCompletionFlowService,
            IAnalyticsService analyticsService)
        {
            _gameService = gameService;
            _levelProgressionService = levelProgressionService;
            _levelCompletionFlowService = levelCompletionFlowService;
            _analyticsService = analyticsService;
        }

        public void Initialize()
        {
            _gameService.OnGameStateChanged += GameService_OnGameStateChanged;
            _levelCompletionFlowService.OnCompletionShown += LevelCompletionFlowService_OnCompletionShown;
            _levelCompletionFlowService.OnReturnToMenuRequested += LevelCompletionFlowService_OnReturnToMenuRequested;
        }

        public void Dispose()
        {
            _gameService.OnGameStateChanged -= GameService_OnGameStateChanged;
            _levelCompletionFlowService.OnCompletionShown -= LevelCompletionFlowService_OnCompletionShown;
            _levelCompletionFlowService.OnReturnToMenuRequested -= LevelCompletionFlowService_OnReturnToMenuRequested;
        }

        public void LogLevelRetryRequested()
        {
            _analyticsService.LogEvent(AnalyticsEvents.LevelRetried, CreateCurrentRunParameters("retry_requested"));
        }

        public void LogLevelReturnToMenuRequested()
        {
            _analyticsService.LogEvent(AnalyticsEvents.LevelReturnedToMenu, CreateCurrentRunParameters("return_to_menu_requested"));
        }

        private void GameService_OnGameStateChanged(object sender, EventArgs e)
        {
            if (_gameService.IsGamePlaying() && !_isRunActive)
            {
                _isRunActive = true;
                _hasLoggedGameOver = false;
                _runStartedAt = Time.realtimeSinceStartup;

                _analyticsService.LogEvent(AnalyticsEvents.LevelStarted, CreateCurrentRunParameters("started"));
                return;
            }

            if (_gameService.IsGameOver() && _isRunActive && !_hasLoggedGameOver)
            {
                _hasLoggedGameOver = true;
                _isRunActive = false;

                _analyticsService.LogEvent(AnalyticsEvents.LevelFailed, CreateCurrentRunParameters("time_expired"));
            }
        }

        private void LevelCompletionFlowService_OnCompletionShown(object sender, LevelCompletionShownEventArgs e)
        {
            var parameters = CreateBaseParameters(e.LevelIndex, "completed");
            parameters[AnalyticsParameters.CompletedOrders] = e.CompletedOrders;
            parameters[AnalyticsParameters.FailedOrders] = e.FailedOrders;

            _isRunActive = false;

            _analyticsService.LogEvent(AnalyticsEvents.LevelCompleted, parameters);
        }

        private void LevelCompletionFlowService_OnReturnToMenuRequested(object sender, EventArgs e)
        {
            LogLevelReturnToMenuRequested();
        }

        private Dictionary<string, object> CreateCurrentRunParameters(string resultReason)
        {
            Dictionary<string, object> parameters = CreateBaseParameters(_levelProgressionService.CurrentLevelIndex, resultReason);
            parameters[AnalyticsParameters.CompletedOrders] = _levelProgressionService.CompletedOrdersInLevel;
            parameters[AnalyticsParameters.FailedOrders] = _levelProgressionService.FailedOrdersInLevel;

            return parameters;
        }

        private Dictionary<string, object> CreateBaseParameters(int levelIndex, string resultReason)
        {
            return new Dictionary<string, object>
            {
                [AnalyticsParameters.LevelIndex] = levelIndex,
                [AnalyticsParameters.RunDurationSec] = GetRunDurationSec(),
                [AnalyticsParameters.CompletedOrders] = 0,
                [AnalyticsParameters.FailedOrders] = 0,
                [AnalyticsParameters.AcceptedOrders] = 0,
                [AnalyticsParameters.SubmissionFailures] = 0,
                [AnalyticsParameters.ContinueUsed] = false,
                [AnalyticsParameters.ResultReason] = resultReason
            };
        }

        private int GetRunDurationSec()
        {
            if (!_isRunActive && _runStartedAt <= 0f)
            {
                return 0;
            }

            return Mathf.Max(0, Mathf.RoundToInt(Time.realtimeSinceStartup - _runStartedAt));
        }
    }
}