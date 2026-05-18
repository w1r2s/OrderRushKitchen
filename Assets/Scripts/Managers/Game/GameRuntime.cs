using Assets.Scripts.Managers.Input;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers.Game
{
    public class GameRuntime : ITickable, IInitializable, IDisposable
    {
        private readonly IGameService _gameService;
        private readonly IInputService _inputService;

        [Inject]
        public GameRuntime(IGameService gameService, IInputService inputService)
        {
            _gameService = gameService;
            _inputService = inputService;
        }
        public void Initialize()
        {
            _inputService.OnPauseAction += InputService_OnPauseAction;
        }

        private void InputService_OnPauseAction(object sender, EventArgs e)
        {
            _gameService.TogglePauseGame();
        }

        public void Tick()
        {
            _gameService.Tick(Time.deltaTime);
        }
        public void Dispose()
        {
            _inputService.OnPauseAction -= InputService_OnPauseAction;
        }
    }
}
