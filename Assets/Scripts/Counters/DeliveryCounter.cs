using Assets.Scripts.Order;
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

        var obj = player.GetObject();

        if (!obj.TryGetPlate(out var plate))
            return;

        if (plate.State != PlateState.Served)
            return;

        if (plate.ResolvedMenuItem == null)
            return;

        if (!_submissionService.TrySubmit(plate.ResolvedMenuItem, out var submitFailReason))
        {
            Debug.Log($"{submitFailReason}");
            return;
        }

        obj = player.RemoveObject();
        Destroy(obj.gameObject);
        Debug.Log($"order delivered");
    }
}