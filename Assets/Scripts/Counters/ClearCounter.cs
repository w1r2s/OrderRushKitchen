using OrderRushKitchen.PlayerControl;
using OrderRushKitchen.Serving;
using System;
using Zenject;

namespace OrderRushKitchen.Counters
{

    public class ClearCounter : BaseCounter
    {
        public event EventHandler OnInvalidAction;

        private PlateAssemblyService _plateAssemblyService;
        private PlateServingService _plateServingService;

        [Inject]
        private void Construct(PlateAssemblyService plateAssemblyService, PlateServingService plateServingService)
        {
            _plateAssemblyService = plateAssemblyService;
            _plateServingService = plateServingService;
        }

        public override void Interact(Player player)
        {
            // �� ������ ����� -> ����� ������� ������
            if (!HasObject)
            {
                if (!player.HasObject)
                    return;

                TryPlaceObjectFromPlayer(player);
                return;
            }

            // �� ������ ���� �������, � ������ ����� -> �������� ������� �� ������
            if (!player.HasObject)
            {
                TryTransferObjectTo(player);
                return;
            }

            if (player.TryGetObjectAs<PlateKitchenObject>(out var playerPlate))
            {
                if (!_plateAssemblyService.TryAddIngredientFrom(playerPlate, this))
                {
                    OnInvalidAction?.Invoke(this, EventArgs.Empty);
                }
                return;
            }

            if (TryGetObjectAs<PlateKitchenObject>(out var counterPlate))
            {
                if (!_plateAssemblyService.TryAddIngredientFrom(counterPlate, player))
                {
                    OnInvalidAction?.Invoke(this, EventArgs.Empty);
                }
            }

        }

        public override bool TryInteractAlternate(Player player)
        {
            if (!TryGetObjectAs<PlateKitchenObject>(out var plate))
                return false;

            if (plate.State == PlateState.Assembly &&
                !_plateServingService.TryServe(plate))
            {
                OnInvalidAction?.Invoke(this, EventArgs.Empty);
            }

            return true;
        }
    }
}
