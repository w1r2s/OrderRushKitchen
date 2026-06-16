using OrderRushKitchen.Menu;

namespace OrderRushKitchen.Order
{
    public class OrderItem
    {
        private readonly MenuItemDefinitionSo _menuItem;
        public MenuItemDefinitionSo MenuItem => _menuItem;
        public bool IsCompleted { get; private set; }
        public OrderItem(MenuItemDefinitionSo menuItem)
        {
            _menuItem = menuItem;
            IsCompleted = false;
        }
        public bool CanBeFulfilledBy(MenuItemDefinitionSo menuItem)
        {
            if (IsCompleted)
            {
                return false;
            }
            if (menuItem == null)
            {
                return false;
            }
            if (MenuItem != menuItem)
            {
                return false;
            }

            return true;
        }
        public bool TryMarkCompleted()
        {
            if (IsCompleted)
                return false;

            IsCompleted = true;
            return true;
        }

        public bool TryMarkIncomplete()
        {
            if (!IsCompleted)
                return false;

            IsCompleted = false;
            return true;
        }
    }
}
