using OrderRushKitchen.Game;
using System.Collections.Generic;
using Zenject;

namespace OrderRushKitchen.Level
{
    public class LevelSceneResetService
    {
        private readonly IGameService _gameService;
        private readonly List<ILevelResettable> _resettables;

        [Inject]
        public LevelSceneResetService(IGameService gameService, List<ILevelResettable> resettables)
        {
            _gameService = gameService;
            _resettables = resettables;
        }
        public void ResetForNextLevel()
        {
            foreach (var resettable in _resettables)
            {
                resettable.ResetForLevelTransition();
            }

            _gameService.RestartLevelRun();
        }
    }
}
