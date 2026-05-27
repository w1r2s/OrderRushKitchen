using Assets.Scripts.Audio;
using System;
using Zenject;

namespace Assets.Scripts.Order.Runtime
{
    public class OrderAudioController : IInitializable, IDisposable
    {
        private readonly IGameplayAudioEventService _gameplayAudio;
        private readonly IOrderService _orderService;

        [Inject]
        public OrderAudioController(IGameplayAudioEventService gameplayAudio, IOrderService orderService)
        {
            _gameplayAudio = gameplayAudio;
            _orderService = orderService;
        }

        public void Initialize()
        {
            _orderService.OnOrderCompleted += OrderService_OnOrderCompleted;
        }
        public void Dispose()
        {
            _orderService.OnOrderCompleted -= OrderService_OnOrderCompleted;
        }

        private void OrderService_OnOrderCompleted(object sender, OrderServiceEventArgs e)
        {
            _gameplayAudio.PlayGlobal(GameplayAudioEvent.OrderCompleted);
        }
    }
}