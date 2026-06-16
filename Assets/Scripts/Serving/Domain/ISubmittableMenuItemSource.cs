using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;

namespace OrderRushKitchen.Serving
{
    public interface ISubmittableMenuItemSource
    {
        bool TryGetMenuItemForSubmit(out MenuItemDefinitionSo menuItem);
    }
}
