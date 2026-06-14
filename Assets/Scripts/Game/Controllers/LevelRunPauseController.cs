using OrderRushKitchen.Level;
using System;
using Zenject;

namespace OrderRushKitchen.Game
{
    public class LevelRunPauseController : IInitializable, IDisposable
    {
        private readonly IGamePauseService _pauseService;
        private readonly IGameService _gameService;
        private readonly ILevelCompletionFlowService _levelCompletionFlowService;

        [Inject]
        public LevelRunPauseController(IGameService gameService, ILevelCompletionFlowService levelCompletionFlowService, IGamePauseService pauseService)
        {
            _gameService = gameService;
            _levelCompletionFlowService = levelCompletionFlowService;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            _gameService.OnGameStateChanged += GameService_OnGameStateChanged;
            _levelCompletionFlowService.OnCompletionShown += LevelCompletionFlowService_OnCompletionShown;
            _levelCompletionFlowService.OnCompletionHidden += LevelCompletionFlowService_OnCompletionHidden;

        }

        private void GameService_OnGameStateChanged(object sender, EventArgs e)
        {
            if (_gameService.IsGameOver())
                _pauseService.AddPause(GamePauseReason.LevelRunEnded);
        }

        private void LevelCompletionFlowService_OnCompletionShown(object sender, EventArgs e)
        {
            _pauseService.AddPause(GamePauseReason.LevelRunEnded);
        }
        private void LevelCompletionFlowService_OnCompletionHidden(object sender, EventArgs e)
        {
            _pauseService.RemovePause(GamePauseReason.LevelRunEnded);
        }
        public void Dispose()
        {
            _gameService.OnGameStateChanged -= GameService_OnGameStateChanged;
            _levelCompletionFlowService.OnCompletionShown -= LevelCompletionFlowService_OnCompletionShown;
            _levelCompletionFlowService.OnCompletionHidden -= LevelCompletionFlowService_OnCompletionHidden;
        }
    }
}