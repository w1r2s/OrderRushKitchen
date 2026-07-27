using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrderRushKitchen.Order
{
    public class OrderGenerationService : IOrderGenerationService
    {
        private readonly MenuItemDatabase _menuDatabase;
        private readonly ICurrentLevelProvider _levelProvider;
        private readonly HashSet<MenuItemDefinitionSo> _previousOrderItems = new();

        private const float SINGLE_DECAY_K = 0.9f;
        private const float MULTI_DECAY_K = 0.35f;
        private const float DIFFICULTY_K = 0.5f;
        private const float LATEST_TIER_ITEM_CHANCE = 0.7f;

        private int _historyLevelNumber = -1;

        public OrderGenerationService(MenuItemDatabase menuDatabase, ICurrentLevelProvider levelConfig)
        {
            _menuDatabase = menuDatabase;
            _levelProvider = levelConfig;
        }

        public IReadOnlyList<MenuItemDefinitionSo> GenerateOrderItems()
        {
            var level = _levelProvider.CurrentLevel;

            ResetHistoryIfLevelChanged(level.levelNumber);

            var menuItems = _menuDatabase.GetAvailableForLevel(level.levelNumber);
            if (menuItems == null || menuItems.Count == 0)
            {
                return null;
            }
            List<MenuItemDefinitionSo> outList;

            var maxItems = Mathf.Clamp(level.maxItemsPerOrder, 1, menuItems.Count);
            var canGenerateMulti = maxItems >= 2;

            if (!canGenerateMulti || !IsMultiItemOrder())
            {
                outList = GenerateSingleItemOrder(menuItems);
            }
            else
            {
                var minMultiItems = Mathf.Clamp(level.minItemsPerOrder, 2, maxItems);
                var itemsToPick = Random.Range(minMultiItems, maxItems + 1);
                outList = GenerateMultiItemOrder(menuItems, itemsToPick);
            }

            if (outList.Count == 0)
            {
                return null;
            }

            RememberOrder(outList);
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
            weightedDict = CreateCandidatePool(weightedDict, itemsToPick);
            weightSum = weightedDict.Sum(item => item.Value);

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
            weightedDict = CreateCandidatePool(weightedDict, itemsToPick);
            weightSum = weightedDict.Sum(item => item.Value);

            var chance = Random.Range(0f, 1f);
            if (chance <= LATEST_TIER_ITEM_CHANCE)
            {
                menuItemsList = SelectFirstItemForMulti(weightedDict, weightSum);

                boostApplied = menuItemsList.Count > 0;
            }
            if (!boostApplied)
            {
                menuItemsList = SelectItemsWeighted(weightedDict, weightSum, itemsToPick);
                return menuItemsList;
            }

            var firstItem = menuItemsList[0];
            if (weightedDict.TryGetValue(firstItem, out var firstItemWeight))
            {
                weightedDict.Remove(firstItem);
                weightSum -= firstItemWeight;
            }

            var otherItems = SelectItemsWeighted(weightedDict, weightSum, itemsToPick - 1);

            if (otherItems.Count != 0)
                menuItemsList.AddRange(otherItems);

            return menuItemsList;
        }

        private bool IsMultiItemOrder()
        {
            var configuredChance = Mathf.Clamp01(_levelProvider.CurrentLevel.multiItemOrderChance);

            if (configuredChance <= 0f)
                return false;

            if (configuredChance >= 1f)
                return true;

            return Random.value < configuredChance;
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

        private Dictionary<MenuItemDefinitionSo, float> CreateCandidatePool(Dictionary<MenuItemDefinitionSo, float> weightedItems, int requiredCount)
        {
            if (_previousOrderItems.Count == 0)
            {
                return new Dictionary<MenuItemDefinitionSo, float>(weightedItems);
            }

            var withoutPreviousOrder = weightedItems
                .Where(item => !_previousOrderItems.Contains(item.Key))
                .ToDictionary(item => item.Key, item => item.Value);

            if (withoutPreviousOrder.Count >= requiredCount)
            {
                return withoutPreviousOrder;
            }

            return new Dictionary<MenuItemDefinitionSo, float>(weightedItems);
        }

        private List<MenuItemDefinitionSo> SelectItemsWeighted(Dictionary<MenuItemDefinitionSo, float> menuItems, float weightSum, int count)
        {
            if (menuItems.Count == 0 || weightSum <= 0f || count <= 0)
            {
                return new List<MenuItemDefinitionSo>();
            }

            var outList = new List<MenuItemDefinitionSo>();
            var remainingItems = new Dictionary<MenuItemDefinitionSo, float>(menuItems);
            var remainingWeightSum = weightSum;

            for (int i = 0; i < count && remainingItems.Count > 0 && remainingWeightSum > 0f; i++)
            {
                var randW = Random.Range(0f, remainingWeightSum);
                var accSum = 0f;
                MenuItemDefinitionSo pickedItem = null;
                var pickedWeight = 0f;

                foreach (var item in remainingItems)
                {
                    accSum += item.Value;
                    if (accSum >= randW)
                    {
                        pickedItem = item.Key;
                        pickedWeight = item.Value;
                        break;
                    }
                }

                if (pickedItem == null)
                {
                    var lastItem = remainingItems.Last();
                    pickedItem = lastItem.Key;
                    pickedWeight = lastItem.Value;
                }

                outList.Add(pickedItem);
                remainingItems.Remove(pickedItem);
                remainingWeightSum -= pickedWeight;
            }

            return outList;
        }

        private List<MenuItemDefinitionSo> SelectFirstItemForMulti(Dictionary<MenuItemDefinitionSo, float> menuItems, float weightSum)
        {
            if (menuItems.Count == 0 || weightSum <= 0f)
            {
                return new List<MenuItemDefinitionSo>();
            }

            var latestUnlockedLevel = menuItems.Keys.Max(item => item.minLevel);
            var latestTierItems = menuItems.Where(pair => pair.Key.minLevel == latestUnlockedLevel).ToDictionary(pair => pair.Key, pair => pair.Value);

            var latestTierWeightSum = latestTierItems.Sum(item => item.Value);

            if (latestTierItems.Count == 0 || latestTierWeightSum <= 0f)
            {
                return new List<MenuItemDefinitionSo>();
            }
            var outList = SelectItemsWeighted(latestTierItems, latestTierWeightSum, 1);
            return outList;
        }

        private void ResetHistoryIfLevelChanged(int levelNumber)
        {
            if (_historyLevelNumber == levelNumber)
            {
                return;
            }

            _historyLevelNumber = levelNumber;
            _previousOrderItems.Clear();
        }

        private void RememberOrder(IReadOnlyList<MenuItemDefinitionSo> orderItems)
        {
            _previousOrderItems.Clear();

            for (int i = 0; i < orderItems.Count; i++)
            {
                var item = orderItems[i];
                if (item != null)
                {
                    _previousOrderItems.Add(item);
                }
            }
        }
    }
}
