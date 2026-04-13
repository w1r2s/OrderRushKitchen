using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "KitchenChaos/Level Definition List")]
    public class LevelDefinitionListSo : ScriptableObject
    {
        public List<LevelDefinitionSo> levels = new();
    }
}
