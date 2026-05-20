using Assets.Scripts.Managers.Input;
using System;
using Zenject;

namespace Assets.Scripts.Managers.Game
{
    public class GameRuntime : ITickable, IInitializable, IDisposable
    {
        private readonly IGameService _gameService;
        private readonly IGameplayInputService _inputService;
        private readonly IGameClock _clock;
        private readonly IGamePauseService _pauseService;

        [Inject]
        public GameRuntime(IGameService gameService, IGameplayInputService inputService, IGameClock clock, IGamePauseService pauseService)
        {
            _gameService = gameService;
            _inputService = inputService;
            _clock = clock;
            _pauseService = pauseService;
        }
        public void Initialize()
        {
            _inputService.OnPauseAction += InputService_OnPauseAction;
        }

        private void InputService_OnPauseAction(object sender, EventArgs e)
        {
            _pauseService.ToggleUserPause();
        }

        public void Tick()
        {
            _gameService.Tick(_clock.DeltaTime);
        }
        public void Dispose()
        {
            _inputService.OnPauseAction -= InputService_OnPauseAction;
        }
    }
}
