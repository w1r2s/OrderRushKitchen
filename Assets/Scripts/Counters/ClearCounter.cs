using Assets.Scripts.Order;
using Assets.Scripts.Serving;
using System;
using Zenject;

public class ClearCounter : BaseCounter
{
    public event EventHandler OnInvalidAction;
    private IMenuItemResolver _menuItemResolver;
    private PlateAssemblyService _plateAssemblyService;

    [Inject]
    private void Construct(IMenuItemResolver menuItemResolver, PlateAssemblyService plateAssemblyService)
    {
        _menuItemResolver = menuItemResolver;
        _plateAssemblyService = plateAssemblyService;
    }
    public override void Interact(Player player)
    {
        // На стойке пусто -> кладём предмет игрока
        if (!HasObject)
        {
            if (!player.HasObject)
                return;

            TryPlaceObjectFromPlayer(player);
            return;
        }

        // На стойке есть предмет, у игрока пусто -> забираем предмет со стойки
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
    public override void InteractAlternate(Player player)
    {
        if (!HasObject)
            return;

        if (TryGetObjectAs<PlateKitchenObject>(out var counterPlate))
        {
            if (counterPlate.State != PlateState.Assembly)
                return;

            var menuItem = _menuItemResolver.TryResolveMenuItem(counterPlate.Ingredients);
            if (menuItem == null)
            {
                OnInvalidAction?.Invoke(this, EventArgs.Empty);
                return;
            }
            if (!counterPlate.TryServe(menuItem))
            {
                OnInvalidAction?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}