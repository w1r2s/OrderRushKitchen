using Assets.Scripts.Level;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Order
{
    public class OrderGenerationService : IOrderGenerationService
    {
        private readonly MenuItemDatabase _menuDatabase;
        private readonly ICurrentLevelProvider _levelProvider;

        private const float SINGLE_DECAY_K = 0.9f;
        private const float MULTI_DECAY_K = 0.35f;
        private const float DIFFICULTY_K = 0.5f;
        private const float CURRENT_LEVEL_ITEM_CHANCE = 0.7f;
        public OrderGenerationService(MenuItemDatabase menuDatabase, ICurrentLevelProvider levelConfig)
        {
            _menuDatabase = menuDatabase;
            _levelProvider = levelConfig;
        }

        public IReadOnlyList<MenuItemDefinitionSo> GenerateOrderItems()
        {
            var level = _levelProvider.CurrentLevel;

            var menuItems = _menuDatabase.GetAvailableForLevel(level.levelNumber);
            if (menuItems == null || menuItems.Count == 0)
            {
                return null;
            }
            List<MenuItemDefinitionSo> outList;

            if (IsMultiItemOrder())
            {
                var minItems = Mathf.Max(1, level.minItemsPerOrder);
                var maxItems = Mathf.Max(minItems, level.maxItemsPerOrder);

                // upper bound exclusive
                int itemsToPick = Random.Range(minItems, maxItems + 1);
                if (itemsToPick == 1)
                {
                    outList = GenerateSingleItemOrder(menuItems);
                    if (outList.Count == 0)
                    {
                        return null;
                    }

                    return outList;
                }

                outList = GenerateMultiItemOrder(menuItems, itemsToPick);
            }
            else
            {
                outList = GenerateSingleItemOrder(menuItems);
            }

            if (outList.Count == 0)
            {
                return null;
            }

            return outList;
        }
        private List<MenuItemDefinitionSo> GenerateSingleItemOrder(IReadOnlyList<MenuItemDefinitionSo> menuItems)
        {
            List<MenuItemDefinitionSo> menuItemList = new();
            int itemsToPick = 1;

            var weightedDict = CalculateWeight(menuItems, SINGLE_DECAY_K, out float weightSum);
            if (weightedDict.Count == 0 || weightSum <= 0)
            {
                return menuItemList;
            }
            menuItemList = SelectItemsWeighted(weightedDict, weightSum, itemsToPick);

            return menuItemList;
        }
        private List<MenuItemDefinitionSo> GenerateMultiItemOrder(IReadOnlyList<MenuItemDefinitionSo> menuItems, int itemsToPick)
        {
            List<MenuItemDefinitionSo> menuItemsList = new();
            var boostApplied = false;

            var weightedDict = CalculateWeight(menuItems, MULTI_DECAY_K, out float weightSum);
            if (weightedDict.Count == 0 || weightSum <= 0)
            {
                return menuItemsList;
            }
            var chance = Random.Range(0f, 1f);
            if (chance <= CURRENT_LEVEL_ITEM_CHANCE)
            {
                menuItemsList = SelectFirstItemForMulti(weightedDict, weightSum);

                boostApplied = menuItemsList.Count > 0;
            }
            if (!boostApplied)
            {
                menuItemsList = SelectItemsWeighted(weightedDict, weightSum, itemsToPick);
                return menuItemsList;
            }
            var otherItems = SelectItemsWeighted(weightedDict, weightSum, itemsToPick - 1);

            if (otherItems.Count != 0)
                menuItemsList.AddRange(otherItems);

            return menuItemsList;
        }

        private bool IsMultiItemOrder()
        {
            var chance = Random.Range(0.0f, 1.0f);
            return chance <= _levelProvider.CurrentLevel.multiItemOrderChance;
        }

        private Dictionary<MenuItemDefinitionSo, float> CalculateWeight(IReadOnlyList<MenuItemDefinitionSo> menuItems, float itemDecayK, out float weightSum)
        {
            var levelNumber = _levelProvider.CurrentLevel.levelNumber;
            Dictionary<MenuItemDefinitionSo, float> outItems = new();
            weightSum = 0;

            foreach (var item in menuItems)
            {
                // насколько сложное блюдо.
                var diffDelta = Mathf.Max(0, item.difficulty - levelNumber);
                var diffPenalty = 1 + diffDelta * DIFFICULTY_K;

                //насколько устаревшее блюдо.
                var levelDist = Mathf.Max(0, levelNumber - item.minLevel);
                var tierFactor = 1 / (1 + levelDist * itemDecayK);

                var effectiveWeight = item.baseSpawnWeight * tierFactor / diffPenalty;
                if (effectiveWeight <= 0)
                {
                    continue;
                }

                outItems[item] = effectiveWeight;

                weightSum += effectiveWeight;
            }

            return outItems;
        }
        private List<MenuItemDefinitionSo> SelectItemsWeighted(Dictionary<MenuItemDefinitionSo, float> menuItems, float weightSum, int count)
        {
            if (menuItems.Count == 0 || weightSum <= 0f || count <= 0)
            {
                return new List<MenuItemDefinitionSo>();
            }
            var outList = new List<MenuItemDefinitionSo>();

            for (int i = 0; i < count; i++)
            {
                var randW = Random.Range(0f, weightSum);
                var accSum = 0f;
                var picked = false;
                MenuItemDefinitionSo lastItem = null;

                foreach (var item in menuItems)
                {
                    lastItem = item.Key;
                    accSum += item.Value;
                    if (accSum >= randW)
                    {
                        outList.Add(item.Key);
                        picked = true;
                        break;
                    }
                }
                if (!picked && lastItem != null)
                {
                    outList.Add(lastItem);
                }
            }
            return outList;
        }
        private List<MenuItemDefinitionSo> SelectFirstItemForMulti(Dictionary<MenuItemDefinitionSo, float> menuItems, float weightSum)
        {
            if (menuItems.Count == 0 || weightSum <= 0f)
            {
                return new List<MenuItemDefinitionSo>();
            }
            var levelNumber = _levelProvider.CurrentLevel.levelNumber;

            var currentLevelItems = menuItems
                .Where(itemKey => itemKey.Key.minLevel == levelNumber)
                .ToDictionary(item => item.Key, itemValue => itemValue.Value);

            var currentLevelWeightSum = currentLevelItems.Sum(item => item.Value);

            if (currentLevelItems.Count == 0 || currentLevelWeightSum <= 0f)
            {
                return new List<MenuItemDefinitionSo>();
            }
            var outList = SelectItemsWeighted(currentLevelItems, currentLevelWeightSum, 1);
            return outList;
        }
    }
}
