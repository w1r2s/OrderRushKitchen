using Assets.Scripts.Order;
using Assets.Scripts.Selection;
using Assets.Scripts.UI;
using System;
using Zenject;

namespace Assets.Scripts.Managers.Game
{
    public class ModalPauseController : IInitializable, IDisposable
    {
        private readonly IGamePauseService _pauseService;
        private readonly IItemSelectionService _itemSelectionService;
        private readonly OptionsUI _optionsUI;
        private readonly OrderDetailsUI _orderDetailsUI;

        [Inject]
        public ModalPauseController(IGamePauseService pauseService, IItemSelectionService itemSelectionService, OptionsUI optionsUI, OrderDetailsUI orderDetailsUI)
        {
            _pauseService = pauseService;
            _itemSelectionService = itemSelectionService;
            _optionsUI = optionsUI;
            _orderDetailsUI = orderDetailsUI;
        }

        public void Initialize()
        {
            _itemSelectionService.OnSelectionOpened += Modal_Opened;
            _optionsUI.Opened += Modal_Opened;
            _orderDetailsUI.Opened += Modal_Opened;

            _itemSelectionService.OnSelectionClosed += Modal_Closed;
            _optionsUI.Closed += Modal_Closed;
            _orderDetailsUI.Closed += Modal_Closed;
        }

        public void Dispose()
        {
            _itemSelectionService.OnSelectionOpened -= Modal_Opened;
            _optionsUI.Opened -= Modal_Opened;
            _orderDetailsUI.Opened -= Modal_Opened;

            _itemSelectionService.OnSelectionClosed -= Modal_Closed;
            _optionsUI.Closed -= Modal_Closed;
            _orderDetailsUI.Closed -= Modal_Closed;
        }

        private void Modal_Opened(object sender, EventArgs e)
        {
            _pauseService.AddPause(GamePauseReason.Modal);
        }

        private void Modal_Closed(object sender, EventArgs e)
        {
            _pauseService.RemovePause(GamePauseReason.Modal);
        }
    }
}
