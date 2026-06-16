using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using System.Collections.Generic;
using System.Linq;

namespace OrderRushKitchen.Menu
{
    public class MenuItemDatabase
    {
        private readonly List<MenuItemDefinitionSo> _items;
        private readonly Dictionary<string, MenuItemDefinitionSo> _itemsByKey;

        public MenuItemDatabase(List<MenuItemDefinitionSo> items)
        {
            _items = (items ?? new List<MenuItemDefinitionSo>())
                .Where(item => item != null)
                .ToList();

            _itemsByKey = _items
                .Where(item => !string.IsNullOrWhiteSpace(item.key))
                .GroupBy(item => item.key)
                .ToDictionary(group => group.Key, group => group.First());
        }

        public IReadOnlyList<MenuItemDefinitionSo> GetAll()
        {
            return _items;
        }

        public bool TryGetByKey(string key, out MenuItemDefinitionSo item)
        {
            return _itemsByKey.TryGetValue(key, out item);
        }
        public IReadOnlyList<MenuItemDefinitionSo> GetAvailableForLevel(int level)
        {
            return _items.Where(item => item.minLevel <= level).ToList();
        }
    }
}
