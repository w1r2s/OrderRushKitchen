using Assets.Scripts.ScriptableObjects;
using System;

namespace Assets.Scripts.Level
{
    public interface ICurrentLevelProvider
    {
        LevelDefinitionSo CurrentLevel {  get; }
        bool TrySetCurrentLevel(int levelNumber);
        bool TryMoveToNextLevel();
        event EventHandler OnCurrentLevelChanged;
    }
}
