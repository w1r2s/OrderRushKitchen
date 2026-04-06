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
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out var plate))
            {
                _deliveryService.DeliverRecipe(plate);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
