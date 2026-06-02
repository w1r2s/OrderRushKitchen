using Assets.Scripts.Navigation;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Game
{
    public class GamePauseActionsController : IInitializable, IDisposable
    {
        private readonly GamePauseUI _gamePauseUI;
        private readonly OptionsUI _optionsUI;
        private readonly INavigationService _navigationService;
        private readonly IGamePauseService _gamePauseService;

        private bool _openedOptionsFromPause;

        [Inject]
        public GamePauseActionsController(GamePauseUI gamePauseUI, OptionsUI optionsUI, INavigationService navigationService, IGamePauseService gamePauseService)
        {
            _gamePauseUI = gamePauseUI;
            _optionsUI = optionsUI;
            _navigationService = navigationService;
            _gamePauseService = gamePauseService;
        }
        public void Initialize()
        {
            _gamePauseUI.ResumeRequested += GamePauseUI_ResumeRequested;
            _gamePauseUI.MainMenuRequested += GamePauseUI_MainMenuRequested;
            _gamePauseUI.OptionsRequested += GamePauseUI_OptionsRequested;
            _optionsUI.Closed += OptionsUI_Closed;
        }
        public void Dispose()
        {
            _gamePauseUI.ResumeRequested -= GamePauseUI_ResumeRequested;
            _gamePauseUI.MainMenuRequested -= GamePauseUI_MainMenuRequested;
            _gamePauseUI.OptionsRequested -= GamePauseUI_OptionsRequested;
            _optionsUI.Closed -= OptionsUI_Closed;
        }

        private void GamePauseUI_ResumeRequested(object sender, EventArgs e)
        {
            _gamePauseService.RemovePause(GamePauseReason.UserPause);
        }

        private void GamePauseUI_MainMenuRequested(object sender, EventArgs e)
        {
           _navigationService.LoadMainMenuAsync(CancellationToken.None).Forget(HandleLoadException);
        }

        private void GamePauseUI_OptionsRequested(object sender, EventArgs e)
        {
            _openedOptionsFromPause = true;
            _gamePauseUI.SetSuppressed(true);
            _optionsUI.Show();
        }

        private void OptionsUI_Closed(object sender, EventArgs e)
        {
            if (!_openedOptionsFromPause)
                return;

            _openedOptionsFromPause = false;
            _gamePauseUI.SetSuppressed(false);
        }

        private static void HandleLoadException(Exception ex)
        {
            if (ex is OperationCanceledException)
                return;

            Debug.LogException(ex);
        }
    }
}
