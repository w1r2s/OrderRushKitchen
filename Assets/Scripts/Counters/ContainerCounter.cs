using OrderRushKitchen.KitchenObjects;
using OrderRushKitchen.PlayerControl;
using OrderRushKitchen.Selection;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Counters
{

    public class ContainerCounter : BaseCounter
    {
        private IItemSelectionService _selectionService;

        [SerializeField] List<KitchenObjectSo> ingredientOptions;

        public event EventHandler OnPlayerGrabbedObject;

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
            _selectionService.OpenIngredientsSelection(player, ingredientOptions);
        }
    }

}
