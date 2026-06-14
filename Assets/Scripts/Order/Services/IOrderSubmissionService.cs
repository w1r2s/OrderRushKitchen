using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;

namespace OrderRushKitchen.Order
{
    public interface IOrderSubmissionService
    {
        bool TrySubmit(MenuItemDefinitionSo menuItem, out OrderSubmissionFailureReason failureReason);
    }
}