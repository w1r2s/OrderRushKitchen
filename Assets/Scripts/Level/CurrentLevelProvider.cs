using Assets.Scripts.ScriptableObjects;
using System;
using System.Linq;
using Zenject;

namespace Assets.Scripts.Level
{
    public class CurrentLevelProvider : ICurrentLevelProvider
    {
        private readonly LevelDatabase _levelDatabase;

        private LevelDefinitionSo _currentLevel;
        public LevelDefinitionSo CurrentLevel => _currentLevel;

        [Inject]
        public CurrentLevelProvider(LevelDatabase levelDatabase)
        {
            _levelDatabase = levelDatabase;
            if (!_levelDatabase.TryGetLevel(1, out var level))
            {
                var firstLevel = _levelDatabase.GetAll()
                    .OrderBy(l => l.levelNumber)
                    .FirstOrDefault();

                if (firstLevel == null)
                {
                    throw new InvalidOperationException(
                        "CurrentLevelProvider: no levels configured in LevelDatabase.");
                }

                level = firstLevel;
            }

            _currentLevel = level;
        }
    }
}