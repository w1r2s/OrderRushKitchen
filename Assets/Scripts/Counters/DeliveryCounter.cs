using Assets.Scripts.Order;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Serving;
using UnityEngine;
using Zenject;

public class DeliveryCounter : BaseCounter
{
    private IOrderSubmissionService _submissionService;

    [Inject]
    public void Construct(IOrderSubmissionService submissionService)
    {
        _submissionService = submissionService;
    }
    public override void Interact(Player player)
    {
        if (!player.HasObject)
            return;
        MenuItemDefinitionSo itemToSubmit = null;
        if (player.TryGetObjectAs<PlateKitchenObject>(out var plate))
        {

            if (plate.State != PlateState.Served)
                return;

            if (plate.ResolvedMenuItem == null)
                return;

            itemToSubmit = plate.ResolvedMenuItem;
        }

        if (player.TryGetObjectAs<ServedMenuItemKitchenObject>(out var servedMenuItem))
        {
            if (servedMenuItem.ServedMenuItem == null)
                return;

            itemToSubmit = servedMenuItem.ServedMenuItem;
        }

        if (itemToSubmit == null)
            return;

        if (!_submissionService.TrySubmit(itemToSubmit, out var submitFailReason))
        {
            Debug.Log($"{submitFailReason}");
            return;
        }
        var obj = player.RemoveObject();
        Destroy(obj.gameObject);
        Debug.Log($"order delivered");
    }
}