using Assets.Scripts.UI;
using System;
using Zenject;

namespace Assets.Scripts.Managers.Game
{
    public class GameOptionsPauseController : IInitializable, IDisposable
    {
        private readonly OptionsUI _optionsUI;
        private readonly IGamePauseService _pauseService;

        public GameOptionsPauseController(OptionsUI optionsUI, IGamePauseService pauseService)
        {
            _optionsUI = optionsUI;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            _pauseService.OnPauseChanged += PauseService_OnPauseChanged;
        }

        public void Dispose()
        {
            _pauseService.OnPauseChanged -= PauseService_OnPauseChanged;
        }

        private void PauseService_OnPauseChanged(object sender, EventArgs e)
        {
            if (!_pauseService.HasPause(GamePauseReason.UserPause))
                _optionsUI.Hide();
        }
    }
}
