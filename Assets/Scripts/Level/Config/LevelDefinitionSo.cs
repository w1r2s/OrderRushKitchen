using UnityEngine;

namespace OrderRushKitchen.Level
{
    [CreateAssetMenu(menuName = "Order Rush Kitchen/Level Definition")]
    public class LevelDefinitionSo : ScriptableObject
    {
        [Header("Identity")]
        public int levelNumber;

        [Header("GamePlay")]

        [Min(1)]
        public int requiredCompletedOrders = 5;

        [Range(0f,1f)]
        public float multiItemOrderChance = 0f;

        [Range(1, 2)]
        public int minItemsPerOrder = 1;

        [Range(1, 2)]
        public int maxItemsPerOrder = 1;

        [Range(1, 3)]
        public int maxActiveOrders = 2;

        [Min(1f)]
        public float levelDurationSeconds = 300f;
    }
}
