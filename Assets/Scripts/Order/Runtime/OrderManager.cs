using UnityEngine;
using Zenject;

namespace Assets.Scripts.Order
{
    public class OrderManager : MonoBehaviour
    {
        private IOrderService _orderService;

        [Inject]
        private void Construct(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private void Update()
        {
            _orderService.Tick(Time.deltaTime);
        }
    }
}
