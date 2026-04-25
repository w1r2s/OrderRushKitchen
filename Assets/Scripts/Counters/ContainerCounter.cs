using Assets.Scripts.Selection;
using System;
using UnityEngine;
using Zenject;

public class ContainerCounter : BaseCounter
{
    private IItemSelectionService _selectionService;

    [SerializeField] private KitchenObjectSo kitchenObjectSo;

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
        _selectionService.OpenIngredientsSelection(player);

        return;

        //if (!player.HasObject)
        //{
        //    player.SpawnAndSet(kitchenObjectSo.prefab);

        //    OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        //}
    }
}
