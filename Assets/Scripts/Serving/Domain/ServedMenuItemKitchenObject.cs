using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Serving
{
    public class ServedMenuItemKitchenObject : KitchenObject, ISubmittableMenuItemSource
    {
        public MenuItemDefinitionSo ServedMenuItem { get; private set; }
        [SerializeField] private Transform contentRoot;
        public bool TryInitialize(MenuItemDefinitionSo menuItem)
        {
            if (menuItem == null)
            {
                return false;
            }

            if (ServedMenuItem != null)
            {
                return false;
            }

            if (menuItem.servedVisualPrefab == null || contentRoot == null)
            {
                return false;
            }
            ServedMenuItem = menuItem;

            Instantiate(ServedMenuItem.servedVisualPrefab, contentRoot, false);

            return true;
        }

        public bool TryGetMenuItemForSubmit(out MenuItemDefinitionSo menuItem)
        {
            menuItem = null;
            if (ServedMenuItem == null)
                return false;

            menuItem = ServedMenuItem;
            return true;
        }
    }
}
