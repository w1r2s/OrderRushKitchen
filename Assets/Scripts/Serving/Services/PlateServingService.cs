using OrderRushKitchen.Order;

namespace OrderRushKitchen.Serving
{
    public class PlateServingService
    {
        private readonly IMenuItemResolver _menuItemResolver;

        public PlateServingService(IMenuItemResolver menuItemResolver)
        {
            _menuItemResolver = menuItemResolver;
        }

        public bool TryServe(PlateKitchenObject plate)
        {
            if (plate == null || plate.State != PlateState.Assembly)
                return false;

            var menuItem = _menuItemResolver.TryResolveMenuItem(plate.Ingredients);
            if (menuItem == null)
                return false;

            return plate.TryServe(menuItem);
        }
    }
}