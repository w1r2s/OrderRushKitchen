using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Serving;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class DrinkCounter : BaseCounter
    {
        private IServedMenuItemFactory _servedItemFactory;

        [SerializeField] private MenuItemDefinitionSo defaultDrinkMenuItem;

        [Inject]
        private void Construct(IServedMenuItemFactory servedItemFactory)
        {
            _servedItemFactory = servedItemFactory;
        }

        public override void Interact(Player player)
        {
            if (player.HasObject)
                return;

            if (defaultDrinkMenuItem == null || defaultDrinkMenuItem.category != MenuItemCategory.Drink)
            {
                Debug.LogWarning("drink counter: default item is null or not drink type");
                return;
            }
            if (!_servedItemFactory.TryCreate(defaultDrinkMenuItem, player, out _))
            {
                Debug.LogWarning("drink counter: create servedItem failed");
                return;
            }
            Debug.Log("drink counter: drink granted");
        }
    }
}
