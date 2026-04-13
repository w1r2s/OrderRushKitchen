using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class LevelDatabase
    {
        private readonly List<LevelDefinitionSo> _levels;
        private readonly Dictionary<int, LevelDefinitionSo> _levelsByNumber;
        public LevelDatabase(List<LevelDefinitionSo> levels)
        {
            _levels = (levels ?? new List<LevelDefinitionSo>())
                .Where(level => level != null)
                .ToList();
            _levelsByNumber = _levels.GroupBy(level => level.levelNumber).ToDictionary(group => group.Key, group => group.First());
        }
        public IReadOnlyList<LevelDefinitionSo> GetAll()
        {
           return _levels;
        }
        public bool TryGetLevel(int levelNumber, out LevelDefinitionSo level)
        {
            return _levelsByNumber.TryGetValue(levelNumber, out level);
        }

    }
}
