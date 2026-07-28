using System;
using Zenject;

namespace OrderRushKitchen.Game
{
    public class GameplayHudVisibilityController : IInitializable, IDisposable
    {
        private readonly IGamePauseService _pauseService;
        private readonly GameplayHudUI _gameplayHudUI;

        public GameplayHudVisibilityController(
            IGamePauseService pauseService,
            GameplayHudUI gameplayHudUI)
        {
            _pauseService = pauseService;
            _gameplayHudUI = gameplayHudUI;
        }

        public void Initialize()
        {
            _pauseService.OnPauseChanged += PauseService_OnPauseChanged;
            RefreshVisibility();
        }

        public void Dispose()
        {
            _pauseService.OnPauseChanged -= PauseService_OnPauseChanged;
        }

        private void PauseService_OnPauseChanged(object sender, EventArgs e)
        {
            RefreshVisibility();
        }

        private void RefreshVisibility()
        {
            _gameplayHudUI.SetVisible(!_pauseService.IsPaused);
        }
    }
}
