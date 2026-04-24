using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Serving
{
    public interface ISubmittableMenuItemSource
    {
        bool TryGetMenuItemForSubmit(out MenuItemDefinitionSo menuItem);
    }
}
