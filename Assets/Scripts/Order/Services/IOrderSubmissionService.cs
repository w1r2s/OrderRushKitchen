using OrderRushKitchen.Menu;

namespace OrderRushKitchen.Order
{
    public interface IOrderSubmissionService
    {
        OrderSubmissionResult TrySubmit(MenuItemDefinitionSo menuItem);
        OrderSubmissionResult TrySubmit(MenuItemDefinitionSo menuItem, ActiveOrder targetOrder);
    }
}
