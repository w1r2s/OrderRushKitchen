using Assets.Scripts.Progress;
using System;
using System.Linq;
using Zenject;

namespace Assets.Scripts.Level
{
    public class LevelStartupInitializer : IInitializable
    {
        private readonly ICurrentLevelProvider _currentLevelProvider;
        private readonly LevelDatabase _levelDatabase;
        private readonly IUserProgressService _userProgressService;
        public LevelStartupInitializer(ICurrentLevelProvider currentLevelProvider, LevelDatabase levelDatabase, IUserProgressService userProgressService)
        {
            _currentLevelProvider = currentLevelProvider;
            _levelDatabase = levelDatabase;
            _userProgressService = userProgressService;
        }
        public void Initialize()
        {
            if (!_currentLevelProvider.TrySetCurrentLevel(_userProgressService.CurrentLevel))
            {

                var firstLevel = _levelDatabase.GetAll()
                    .OrderBy(l => l.levelNumber)
                    .FirstOrDefault();

                if (firstLevel == null)
                {
                    throw new InvalidOperationException("LevelStartupInitializer: no levels configured in LevelDatabase.");
                }

                if (!_currentLevelProvider.TrySetCurrentLevel(firstLevel.levelNumber))
                {
                    throw new InvalidOperationException("LevelStartupInitializer: couldn't initialize level");
                }
            }
        }
    }
}
