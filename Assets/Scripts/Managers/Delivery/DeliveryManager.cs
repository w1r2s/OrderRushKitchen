using UnityEngine;
using Zenject;

public class DeliveryManager : MonoBehaviour
{
    private IDeliveryService _deliveryService;

    [Inject]
    public void Construct(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }
    private void Update()
    {
        _deliveryService.Tick(Time.deltaTime);
    }
}