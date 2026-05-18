using UnityEngine;
using Zenject;

namespace Assets.Scripts.Order.Runtime
{
    internal class OrderRuntime : ITickable
    {
        private readonly IOrderService _orderService;
        
        [Inject]
        public OrderRuntime(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public void Tick()
        {
            _orderService.Tick(Time.deltaTime);
        }
    }
}
