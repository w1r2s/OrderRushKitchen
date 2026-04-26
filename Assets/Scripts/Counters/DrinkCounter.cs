using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Selection;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Counters
{
    public class DrinkCounter : BaseCounter
    {
        private IItemSelectionService _selectionService;

        [SerializeField] private List<MenuItemDefinitionSo> drinkOptions;

        [Inject]
        private void Construct(IItemSelectionService selectionService)
        {
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
            _selectionService.OpenDrinksSelection(player, drinkOptions);
        }
    }
}
