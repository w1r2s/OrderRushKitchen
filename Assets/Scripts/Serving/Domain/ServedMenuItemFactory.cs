using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.Level;
using OrderRushKitchen.Menu;
using UnityEngine;

namespace OrderRushKitchen.Serving
{
    public class ServedMenuItemFactory : IServedMenuItemFactory
    {
        private readonly KitchenObjectSo _servedItemPrefab;

        public ServedMenuItemFactory(KitchenObjectSo servedItemPrefab)
        {
            _servedItemPrefab = servedItemPrefab;
        }

        public bool TryCreate(MenuItemDefinitionSo menuItem, ObjectHolder holder, out ServedMenuItemKitchenObject createdItem)
        {
            createdItem = null;
            if (menuItem == null || holder == null || holder.HasObject)
                return false;

            if (_servedItemPrefab == null || _servedItemPrefab.prefab == null)
                return false;

            var spawned = Object.Instantiate(_servedItemPrefab.prefab);

            if (spawned is not ServedMenuItemKitchenObject servedItem)
            {
                Object.Destroy(spawned.gameObject);
                return false;
            }

            if (!servedItem.TryInitialize(menuItem))
            {
                Object.Destroy(servedItem.gameObject);
                return false;
            }

            if (!holder.TrySetObject(servedItem))
            {
                Object.Destroy(servedItem.gameObject);
                return false;
            }

            createdItem = servedItem;
            return true;
        }
    }
}
