using Assets.Scripts.Level;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Order
{
    public class MenuItemResolver : IMenuItemResolver
    {
        private readonly MenuItemDatabase _menuDatabase;
        private readonly ICurrentLevelProvider _levelProvider;
        public MenuItemResolver(MenuItemDatabase menuDatabase, ICurrentLevelProvider levelProvider)
        {
            _menuDatabase = menuDatabase;
            _levelProvider = levelProvider;
        }
        public MenuItemDefinitionSo TryResolveMenuItem(IReadOnlyList<KitchenObjectSo> kitchenObjects)
        {
            if (kitchenObjects == null || kitchenObjects.Count == 0)
                return null;

            var menuItems = _menuDatabase.GetAvailableForLevel(_levelProvider.CurrentLevel.levelNumber);
            if (menuItems.Count == 0)
                return null;

            var deliveredCounts = BuildCounts(kitchenObjects);

            foreach (var menuItem in menuItems)
            {
                if (menuItem == null)
                    continue;

                var required = menuItem.requiredIngredients;
                if (required == null || required.Count != kitchenObjects.Count)
                    continue;

                var requiredCounts = BuildCounts(required);

                if (AreCountsEqual(requiredCounts, deliveredCounts))
                    return menuItem;

            }
            return null;
        }
        private Dictionary<KitchenObjectSo, int> BuildCounts(IReadOnlyList<KitchenObjectSo> items)
        {
            var counts = new Dictionary<KitchenObjectSo, int>();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item == null) continue;

                if (counts.TryGetValue(item, out var value))
                    counts[item] = value + 1;
                else
                    counts[item] = 1;
            }
            return counts;
        }
        private bool AreCountsEqual(Dictionary<KitchenObjectSo, int> left, Dictionary<KitchenObjectSo, int> right)
        {
            if (left.Count != right.Count)
                return false;

            foreach (var keyValuePair in left)
            {
                if (!right.TryGetValue(keyValuePair.Key, out var rightCount))
                    return false;

                if (keyValuePair.Value != rightCount)
                    return false;
            }

            return true;
        }

    }
}