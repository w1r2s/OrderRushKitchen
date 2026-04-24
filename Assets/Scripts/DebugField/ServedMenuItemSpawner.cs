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
        private IServedMenuItemFactory _servedItemFactory;

        [Inject]
        private void Construct(Player player, MenuItemDatabase menuDataBase, IServedMenuItemFactory servedItemFactory)
        {
            _menuDataBase = menuDataBase;
            _player = player;
            _servedItemFactory = servedItemFactory;
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

                if (_servedItemFactory.TryCreate(drinkItem, _player, out _))
                {
                    Debug.Log("served spawn Success");
                    return;
                }

                Debug.Log("served spawn failed");
            }
        }
    }
}
