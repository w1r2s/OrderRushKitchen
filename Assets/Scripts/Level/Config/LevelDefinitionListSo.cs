using System.Collections.Generic;
using UnityEngine;

namespace OrderRushKitchen.Level
{
    [CreateAssetMenu(menuName = "Order Rush Kitchen/Level Definition List")]
    public class LevelDefinitionListSo : ScriptableObject
    {
        public List<LevelDefinitionSo> levels = new();
    }
}
