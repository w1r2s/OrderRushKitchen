using Assets.Scripts.Order;
using Assets.Scripts.Serving;
using System;
using UnityEngine;
using Zenject;

public class DeliveryCounter : BaseCounter
{
    public event EventHandler OnDeliverySuccess;
    public event EventHandler OnDeliveryFail;

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
        {
            OnDeliveryFail?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (!source.TryGetMenuItemForSubmit(out var menuItem) || menuItem == null)
        {
            OnDeliveryFail?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (!_submissionService.TrySubmit(menuItem, out var submitFailReason))
        {
            OnDeliveryFail?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (!player.TryRemoveAndDestroyObject())
        {
            Debug.LogError($"{name}: submitted object could not be removed from player");
        }

        OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
    }
}