using Assets.Scripts.Serving;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.DebugField
{
    public class ServedMenuItemSpawner : MonoBehaviour
    {
        private MenuItemDatabase _menuDataBase;
        private Player _player;
        [SerializeField] private ServedMenuItemKitchenObject servedItemPrefab;

        [Inject]
        private void Construct(Player player, MenuItemDatabase menuDataBase)
        {
            _menuDataBase = menuDataBase;
            _player = player;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (_player.HasObject)
                    return;

                var drinkItem = _menuDataBase.GetAvailableForLevel(1).FirstOrDefault(i => i.category == ScriptableObjects.MenuItemCategory.Drink);
                if (drinkItem == null)
                {
                    Debug.Log("no served item found");
                    return;
                }
                if (servedItemPrefab == null)
                {
                    Debug.Log("spawner prefab not found");
                    return;
                }
                var servedItem = Instantiate(servedItemPrefab);
                if (servedItem.TryInitialize(drinkItem))
                {
                    _player.SetObject(servedItem);
                    Debug.Log("served spawn Success");
                    return;
                }
              
                Debug.Log("served spawn failed");
                Destroy(servedItem);
            }
        }
    }
}
