using Assets.Scripts.Order;
using Assets.Scripts.Serving;
using System;
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
        var removed = player.RemoveObject();
        Destroy(removed.gameObject);
        OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
    }
}