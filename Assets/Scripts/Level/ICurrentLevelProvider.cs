using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Level
{
    public interface ICurrentLevelProvider
    {
        LevelDefinitionSo CurrentLevel {  get; }
    }
}
