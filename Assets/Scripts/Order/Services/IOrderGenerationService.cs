using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using System.Collections.Generic;

namespace OrderRushKitchen.Order
{
    public interface IOrderGenerationService
    {
        IReadOnlyList<MenuItemDefinitionSo> GenerateOrderItems();
    }
}
