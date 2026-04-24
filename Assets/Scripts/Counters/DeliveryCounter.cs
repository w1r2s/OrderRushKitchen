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

        if (obj is not ISubmittableMenuItemSource source)
            return;

        if (!source.TryGetMenuItemForSubmit(out var menuItem) || menuItem == null)
            return;

        if (!_submissionService.TrySubmit(menuItem, out var submitFailReason))
        {
            Debug.Log($"{submitFailReason}");
            return;
        }
        var removed = player.RemoveObject();
        Destroy(removed.gameObject);
        Debug.Log($"order delivered");
    }
}