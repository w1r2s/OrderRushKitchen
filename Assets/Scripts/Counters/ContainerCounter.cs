using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSo kitchenObjectSo;

    public event EventHandler OnPlayerGrabbedObject;
    public override void Interact(Player player)
    {
        if (!player.HasObject)
        {
            player.SpawnAndSet(kitchenObjectSo.prefab);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
