using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "KitchenChaos/Level Definition")]
    public class LevelDefinitionSo : ScriptableObject
    {
        [Header("Identity")]
        public int levelNumber;

        [Header("GamePlay")]

        [Min(1)]
        public int requiredCompletedOrders = 5;

        [Min(0f)]
        public float multiItemOrderChance = 0f;

        [Range(1, 3)]
        public int minItemsPerOrder = 1;

        [Range(1, 3)]
        public int maxItemsPerOrder = 1;

        [Range(0.1f, 3f)]
        public float satisfactionDrainMultiplier = 1f;

        [Range(1, 4)]
        public int maxActiveOrders = 2;
    }
}
