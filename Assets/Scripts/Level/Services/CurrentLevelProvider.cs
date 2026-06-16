using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Menu;
using System;
using Zenject;

namespace OrderRushKitchen.Level
{
    public class CurrentLevelProvider : ICurrentLevelProvider
    {
        private readonly LevelDatabase _levelDatabase;

        private LevelDefinitionSo _currentLevel;

        public event EventHandler OnCurrentLevelChanged;

        public LevelDefinitionSo CurrentLevel => _currentLevel;

        [Inject]
        public CurrentLevelProvider(LevelDatabase levelDatabase)
        {
            _levelDatabase = levelDatabase;
        }

        public bool TrySetCurrentLevel(int levelNumber)
        {

            if (levelNumber <= 0)
                return false;

            if (_currentLevel != null && _currentLevel.levelNumber == levelNumber)
                return false;


            if (!_levelDatabase.TryGetLevel(levelNumber, out var level))
            {
                return false;
            }
            _currentLevel = level;
            OnCurrentLevelChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool TryMoveToNextLevel()
        {
            if (_currentLevel == null)
                return false;

            if (!_levelDatabase.TryGetLevel(_currentLevel.levelNumber + 1, out var level))
            {
                return false;
            }
            _currentLevel = level;
            OnCurrentLevelChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }
}