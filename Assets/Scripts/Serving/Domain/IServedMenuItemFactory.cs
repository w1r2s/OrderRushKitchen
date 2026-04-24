using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Serving
{
    public interface IServedMenuItemFactory
    {
        bool TryCreate(MenuItemDefinitionSo menuItem, ObjectHolder holder, out ServedMenuItemKitchenObject createdItem);
    }
}
