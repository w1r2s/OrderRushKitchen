using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Order
{
    public class OrderCreationService : IOrderCreationService
    {
        private readonly MenuItemDatabase _menuDatabase;
        private readonly IOrderService _orderService;
        private readonly LevelDefinitionSo _levelConfig;
        public OrderCreationService(MenuItemDatabase menuDatabase, IOrderService orderService, LevelDefinitionSo levelDefinitionSo)
        {
            _menuDatabase = menuDatabase;
            _orderService = orderService;
            _levelConfig = levelDefinitionSo;
        }

        public bool TryCreateOrder(out ActiveOrder order)
        {
            var menuItems = _menuDatabase.GetAvailableForLevel(_levelConfig.levelNumber);

            order = null;
            if (menuItems.Count == 0)
            {
                return false;
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
            IReadOnlyList<MenuItemDefinitionSo> menuItemList = SelectRandomItems(menuItems, itemsToPick);

            if (menuItemList.Count == 0)
            {
                return false;
            }

            order = _orderService.CreateOrder(menuItemList);

            if (order == null)
            {
                return false;
            }
            return true;
        }
        private IReadOnlyList<MenuItemDefinitionSo> SelectRandomItems(IReadOnlyList<MenuItemDefinitionSo> menuItems, int count)
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
