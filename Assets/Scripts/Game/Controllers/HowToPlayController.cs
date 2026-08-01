using OrderRushKitchen.Input;
using OrderRushKitchen.UserProgress;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Game
{
    public class HowToPlayController : IInitializable, IDisposable
    {
        private const int FirstLevelNumber = 1;

        private readonly HowToPlayUI _howToPlayUI;
        private readonly GamePauseUI _gamePauseUI;
        private readonly IGamePauseService _pauseService;
        private readonly IUserProgressService _userProgressService;
        private readonly IInputRebindingService _inputRebindingService;

        private HowToPlayOpenMode _openMode;
        private bool _ownsModalPause;

        public HowToPlayController(
            HowToPlayUI howToPlayUI,
            GamePauseUI gamePauseUI,
            IGamePauseService pauseService,
            IUserProgressService userProgressService,
            IInputRebindingService inputRebindingService)
        {
            _howToPlayUI = howToPlayUI;
            _gamePauseUI = gamePauseUI;
            _pauseService = pauseService;
            _userProgressService = userProgressService;
            _inputRebindingService = inputRebindingService;
        }

        public void Initialize()
        {
            _howToPlayUI.ActionRequested += HowToPlayUI_ActionRequested;
            _gamePauseUI.HowToPlayRequested += GamePauseUI_HowToPlayRequested;

            if (_userProgressService.CurrentLevel == FirstLevelNumber &&
                !_userProgressService.HasSeenHowToPlay)
            {
                Open(HowToPlayOpenMode.Automatic);
            }
        }

        public void Dispose()
        {
            _howToPlayUI.ActionRequested -= HowToPlayUI_ActionRequested;
            _gamePauseUI.HowToPlayRequested -= GamePauseUI_HowToPlayRequested;

            if (_ownsModalPause)
                _pauseService.RemovePause(GamePauseReason.Modal);
        }

        private void GamePauseUI_HowToPlayRequested(object sender, EventArgs e)
        {
            Open(HowToPlayOpenMode.FromPause);
        }

        private void HowToPlayUI_ActionRequested(object sender, EventArgs e)
        {
            if (_openMode == HowToPlayOpenMode.Automatic)
                _userProgressService.MarkHowToPlaySeen();

            Close();
        }

        private void Open(HowToPlayOpenMode mode)
        {
            if (_howToPlayUI.IsOpen)
                return;

            _openMode = mode;

            if (mode == HowToPlayOpenMode.FromPause)
                _gamePauseUI.SetSuppressed(true);

            if (!Application.isMobilePlatform)
                RefreshDesktopBindings();

            _pauseService.AddPause(GamePauseReason.Modal);
            _ownsModalPause = true;
            _howToPlayUI.Show(mode);
        }

        private void Close()
        {
            _howToPlayUI.Hide();

            if (_ownsModalPause)
            {
                _pauseService.RemovePause(GamePauseReason.Modal);
                _ownsModalPause = false;
            }

            if (_openMode == HowToPlayOpenMode.FromPause)
                _gamePauseUI.SetSuppressed(false);
        }

        private void RefreshDesktopBindings()
        {
            _howToPlayUI.SetDesktopBindings(
                _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Up),
                _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Down),
                _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Left),
                _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Right),
                _inputRebindingService.GetKeyBindingText(InputKeyBinding.Interact),
                _inputRebindingService.GetKeyBindingText(InputKeyBinding.Alt_Interact));
        }
    }
}
