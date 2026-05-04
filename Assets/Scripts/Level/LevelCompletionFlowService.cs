using Assets.Scripts.Order;
using System;
using Zenject;

namespace Assets.Scripts.Level
{
    public class LevelCompletionFlowService : ILevelCompletionFlowService, IInitializable, IDisposable
    {
        private ILevelProgressionService _progressionService;
        private IOrderService _orderService;

        public event EventHandler<LevelCompletionShownEventArgs> OnCompletionShown;
        public event EventHandler OnCompletionHidden;
        public event EventHandler OnReturnToMenuRequested;

        public bool IsCompletionOpen { get; private set; }

        [Inject]
        public LevelCompletionFlowService(ILevelProgressionService progressionService, IOrderService orderService)
        {
            _progressionService = progressionService;
            _orderService = orderService;
        }
        private void ProgressionService_OnLevelCompleted(object sender, EventArgs e)
        {
            IsCompletionOpen = true;

            OnCompletionShown?.Invoke(this, new LevelCompletionShownEventArgs(_progressionService.CurrentLevelIndex,
                _progressionService.CompletedOrdersInLevel,
                _progressionService.OrdersToCompleteForCurrentLevel));
        }

        public void ConfirmNextLevel()
        {
            if (!IsCompletionOpen)
                return;

            if (_progressionService.TryAdvanceToNextLevel())
            {
                _orderService.ClearAllOrders();
                IsCompletionOpen = false;
                OnCompletionHidden?.Invoke(this, EventArgs.Empty);
                return;
            }
            // не нашли следующий уровень.

        }

        public void RequestReturnToMenu()
        {
            if (!IsCompletionOpen)
                return;

            IsCompletionOpen = false;
            OnCompletionHidden?.Invoke(this, EventArgs.Empty);

            OnReturnToMenuRequested?.Invoke(this, EventArgs.Empty);
        }

        public void Initialize()
        {
            _progressionService.OnLevelCompleted += ProgressionService_OnLevelCompleted;
        }

        public void Dispose()
        {
            if( _progressionService != null )
            _progressionService.OnLevelCompleted -= ProgressionService_OnLevelCompleted;
        }
    }
}
