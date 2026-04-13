using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Order
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
        public void MarkCompleted()
        {
            IsCompleted = true;
        }
    }
}
