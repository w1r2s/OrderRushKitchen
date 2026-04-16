using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Order
{
    public class OrderGenerationService : IOrderGenerationService
    {
        private readonly MenuItemDatabase _menuDatabase;
        private readonly LevelDefinitionSo _levelConfig;

        public OrderGenerationService(MenuItemDatabase menuDatabase, LevelDefinitionSo levelConfig)
        {
            _menuDatabase = menuDatabase;
            _levelConfig = levelConfig;
        }

        public IReadOnlyList<MenuItemDefinitionSo> GenerateOrderItems()
        {
            var menuItems = _menuDatabase.GetAvailableForLevel(_levelConfig.levelNumber);
            if (menuItems.Count == 0)
            {
                return null;
            }

            int itemsToPick;
            if (IsMultiItemOrder())
            {
                var minItems = Mathf.Max(1, _levelConfig.minItemsPerOrder);
                var maxItems = Mathf.Max(minItems, _levelConfig.maxItemsPerOrder);

                // upper bound exclusive
                itemsToPick = Random.Range(minItems, maxItems + 1);
            }
            else
            {
                itemsToPick = 1;
            }

            if (itemsToPick <= 0)
            {
                return null;
            }

            var weightedDict = CalculateWeight(menuItems, out var weightSum);
            if (weightedDict == null || weightedDict.Count == 0)
            {
                return null;
            }

            var menuItemList = SelectItemsWeighted(weightedDict, weightSum, itemsToPick);
            if (menuItemList == null || menuItemList.Count == 0)
            {
                return null;
            }

            return menuItemList;
        }

        private bool IsMultiItemOrder()
        {
            var chance = Random.Range(0.0f, 1.0f);
            return chance <= _levelConfig.multiItemOrderChance;
        }

        private Dictionary<MenuItemDefinitionSo, float> CalculateWeight(IReadOnlyList<MenuItemDefinitionSo> menuItems, out float weightSum)
        {
            Dictionary<MenuItemDefinitionSo, float> outItems = new();
            var k = 0.5f;
            weightSum = 0;

            foreach (var item in menuItems)
            {
                var difficultyPenalty = Mathf.Max(0, item.difficulty - _levelConfig.levelNumber);
                var effectiveWeight = item.baseSpawnWeight / (1f + difficultyPenalty * k);
                outItems.Add(item, effectiveWeight);
                weightSum += effectiveWeight;
            }

            if (weightSum <= 0)
                return null;

            return outItems;
        }
        private IReadOnlyList<MenuItemDefinitionSo> SelectItemsWeighted(Dictionary<MenuItemDefinitionSo, float> menuItems, float weightSum, int count)
        {
            if (menuItems == null || menuItems.Count == 0 || weightSum <= 0f || count <= 0)
            {
                return null;
            }

            var outList = new List<MenuItemDefinitionSo>();

            for (int i = 0; i < count; i++)
            {
                var randW = Random.Range(0, weightSum);
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
    }
}
