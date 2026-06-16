using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;

namespace OrderRushKitchen.Serving
{
    public interface IServedMenuItemFactory
    {
        bool TryCreate(MenuItemDefinitionSo menuItem, ObjectHolder holder, out ServedMenuItemKitchenObject createdItem);
    }
}
