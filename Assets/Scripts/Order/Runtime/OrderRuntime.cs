using Assets.Scripts.Managers.Game;
using Zenject;

namespace Assets.Scripts.Order.Runtime
{
    internal class OrderRuntime : ITickable
    {
        private readonly IOrderService _orderService;
        private readonly IGameClock _clock;

        [Inject]
        public OrderRuntime(IOrderService orderService, IGameClock clock)
        {
            _orderService = orderService;
            _clock = clock;
        }
        public void Tick()
        {
            _orderService.Tick(_clock.DeltaTime);
        }
    }
}
