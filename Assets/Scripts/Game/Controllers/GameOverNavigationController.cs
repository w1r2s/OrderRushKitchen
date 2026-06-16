using Cysharp.Threading.Tasks;
using OrderRushKitchen.Navigation;
using System.Threading;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Game
{
    public class GameOverNavigationController : IInitializable, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly GameOverUI _gameOverUI;

        public GameOverNavigationController(INavigationService navigationService, GameOverUI gameOverUI)
        {
            _navigationService = navigationService;
            _gameOverUI = gameOverUI;
        }

        public void Initialize()
        {
            _gameOverUI.RetryRequested += GameOverUI_RetryRequested;
            _gameOverUI.MainMenuRequested += GameOverUI_MainMenuRequested;

        }

        public void Dispose()
        {
            _gameOverUI.RetryRequested -= GameOverUI_RetryRequested;
            _gameOverUI.MainMenuRequested -= GameOverUI_MainMenuRequested;
        }

        private void GameOverUI_MainMenuRequested(object sender, EventArgs e)
        {
            _navigationService.LoadMainMenuAsync(CancellationToken.None).Forget(HandleLoadException);
        }

        private void GameOverUI_RetryRequested(object sender, EventArgs e)
        {
            _navigationService.ReloadGameAsync(CancellationToken.None).Forget(HandleLoadException);
        }

        private void HandleLoadException(Exception ex)
        {
            if (ex is OperationCanceledException)
                return;

            Debug.LogException(ex);
        }
    }
}
