using Assets.Scripts.Selection;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
