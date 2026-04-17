using Assets.Scripts.Order;
using UnityEngine;
using Zenject;

public class DeliveryCounter : BaseCounter
{
    private IOrderSubmissionService _submissionService;
    private IMenuItemResolver _menuItemResolver;
    [Inject]
    public void Construct(IOrderSubmissionService submissionService, IMenuItemResolver menuItemResolver)
    {
        _submissionService = submissionService;
        _menuItemResolver = menuItemResolver;
    }
    public override void Interact(Player player)
    {
        if (!player.HasObject)
            return;

        var obj = player.GetObject();

        if (!obj.TryGetPlate(out var plate))
            return;

        var itemsCandidates = plate.GetKitchenObjectSoList();
        var menuCandidate = _menuItemResolver.TryResolveMenuItem(itemsCandidates);

        if (!_submissionService.TrySubmit(menuCandidate, out var submitFailReason))
        {
            Debug.Log($"{submitFailReason}");
            return;
        }

        obj = player.RemoveObject();
        Destroy(obj.gameObject);
        Debug.Log($"order delivered");
    }
}