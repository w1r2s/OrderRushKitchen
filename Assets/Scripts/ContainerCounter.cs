using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSo kitchenObjectSo;

    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSo, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
