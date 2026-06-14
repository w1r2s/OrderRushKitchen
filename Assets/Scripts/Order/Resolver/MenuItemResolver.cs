using OrderRushKitchen.Composition;
using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;

namespace OrderRushKitchen.Order
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

            var deliveredCounts = IngredientComposition.BuildCounts(kitchenObjects);

            foreach (var menuItem in menuItems)
            {
                if (menuItem == null)
                    continue;

                var required = menuItem.requiredIngredients;
                if (required == null || required.Count != kitchenObjects.Count)
                    continue;

                var requiredCounts = IngredientComposition.BuildCounts(required);

                if (IngredientComposition.AreEqual(requiredCounts, deliveredCounts))
                    return menuItem;

            }
            return null;
        }
    }
}