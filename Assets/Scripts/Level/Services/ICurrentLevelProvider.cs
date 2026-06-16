using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Menu;
using System;

namespace OrderRushKitchen.Level
{
    public interface ICurrentLevelProvider
    {
        LevelDefinitionSo CurrentLevel {  get; }
        bool TrySetCurrentLevel(int levelNumber);
        bool TryMoveToNextLevel();
        event EventHandler OnCurrentLevelChanged;
    }
}
