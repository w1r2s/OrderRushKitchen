using Zenject;

public class DeliveryCounter : BaseCounter
{
    private IDeliveryService _deliveryService;

    [Inject]
    public void Construct(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }
    public override void Interact(Player player)
    {
        if (!player.HasObject)
            return;

        var obj = player.GetObject();

        if (!obj.TryGetPlate(out var plate))
            return;

        _deliveryService.DeliverRecipe(plate);

        obj = player.RemoveObject();
        Destroy(obj.gameObject);
    }
}
