using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Selection;
using Assets.Scripts.Serving;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class DrinkCounter : BaseCounter
    {
        private IServedMenuItemFactory _servedItemFactory;
        private IItemSelectionService _selectionService;

        [SerializeField] private MenuItemDefinitionSo defaultDrinkMenuItem;

        [Inject]
        private void Construct(IServedMenuItemFactory servedItemFactory, IItemSelectionService selectionService)
        {
            _servedItemFactory = servedItemFactory;
            _selectionService = selectionService;
        }

        public override void Interact(Player player)
        {
            if (player.HasObject)
                return;

            if (_selectionService.IsOpen)
            {
                _selectionService.CloseSelection();
                return;
            }
            _selectionService.OpenDrinksSelection(player);
            return;
            //

            //if (defaultDrinkMenuItem == null || defaultDrinkMenuItem.category != MenuItemCategory.Drink)
            //{
            //    Debug.LogWarning("drink counter: default item is null or not drink type");
            //    return;
            //}
            //if (!_servedItemFactory.TryCreate(defaultDrinkMenuItem, player, out _))
            //{
            //    Debug.LogWarning("drink counter: create servedItem failed");
            //    return;
            //}
            //Debug.Log("drink counter: drink granted");
        }
    }
}
