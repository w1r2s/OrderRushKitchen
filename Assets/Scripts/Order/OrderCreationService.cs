using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Order
{
    public class OrderCreationService : IOrderCreationService
    {
        private readonly MenuItemDatabase _menuDatabase;
        private readonly IOrderService _orderService;

        private const int MAX_ITEMS_IN_ORDER = 3;
        public OrderCreationService(MenuItemDatabase menuDatabase, IOrderService orderService)
        {
            _menuDatabase = menuDatabase;
            _orderService = orderService;
        }

        public bool TryCreateOrder(out ActiveOrder order)
        {
            var menuItems = _menuDatabase.GetAll();

            order = null;
            if (menuItems.Count == 0)
            {
                return false;
            }
            int itemsToPick = UnityEngine.Random.Range(1, MAX_ITEMS_IN_ORDER + 1); // + 1 because of exclusive upper range

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

    }
}
