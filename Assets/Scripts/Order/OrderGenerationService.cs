using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Order
{
    //TODO: Apply menuItem weight collection into generation rules, aplly level restrictions into generation.
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
                itemsToPick = UnityEngine.Random.Range(_levelConfig.minItemsPerOrder, _levelConfig.maxItemsPerOrder + 1); // + 1 because of exclusive upper range
            }
            else
            {
                itemsToPick = 1;
            }

            var menuItemList = SelectMenuItems(menuItems, itemsToPick);
            if (menuItemList.Count == 0)
            {
                return null;
            }

            return menuItemList;
        }
        private IReadOnlyList<MenuItemDefinitionSo> SelectMenuItems(IReadOnlyList<MenuItemDefinitionSo> menuItems, int count)
        {
            var outList = new List<MenuItemDefinitionSo>();
            for (int i = 0; i < count; i++)
            {
                var randomItem = menuItems[UnityEngine.Random.Range(0, menuItems.Count)];
                outList.Add(randomItem);
            }

            return outList;
        }
        private bool IsMultiItemOrder()
        {
            var chance = UnityEngine.Random.Range(0.0f, 1.0f);

            if (chance <= _levelConfig.multiItemOrderChance)
            {

                return true;
            }

            return false;
        }


    }
}
