using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;

namespace Assets.Scripts.Order
{
    public interface IMenuItemResolver
    {
        MenuItemDefinitionSo TryResolveMenuItem(IReadOnlyList<KitchenObjectSo> kitchenObjects);
    }
}
