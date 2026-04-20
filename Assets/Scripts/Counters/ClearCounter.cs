using Assets.Scripts.Order;
using Assets.Scripts.Serving;
using UnityEngine;
using Zenject;

public class ClearCounter : BaseCounter
{
    private IMenuItemResolver _menuItemResolver;

    [Inject]
    private void Construct(IMenuItemResolver menuItemResolver)
    {
        _menuItemResolver = menuItemResolver;
    }
    public override void Interact(Player player)
    {
        // На стойке пусто -> кладём предмет игрока
        if (!HasObject)
        {
            if (!player.HasObject)
                return;

            PlaceObjectFromPlayer(player);
            return;
        }

        // На стойке есть предмет, у игрока пусто -> забираем предмет со стойки
        if (!player.HasObject)
        {
            KitchenObject counterObject = RemoveObject();
            player.SetObject(counterObject);
            return;
        }

        // И у игрока, и у стойки есть предметы -> пробуем объединить с тарелкой
        KitchenObject playerObjectInHand = player.GetObject();
        KitchenObject counterObjectOnCounter = GetObject();

        if (playerObjectInHand.TryGetPlate(out PlateKitchenObject playerPlate))
        {
            if (playerPlate.TryAddIngredient(counterObjectOnCounter.KitchenObjectSo))
            {
                KitchenObject removedObject = RemoveObject();
                Destroy(removedObject.gameObject);
            }

            return;
        }

        if (counterObjectOnCounter.TryGetPlate(out PlateKitchenObject counterPlate))
        {
            if (counterPlate.TryAddIngredient(playerObjectInHand.KitchenObjectSo))
            {
                KitchenObject removedObject = player.RemoveObject();
                Destroy(removedObject.gameObject);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (!HasObject)
            return;

        KitchenObject counterObjectOnCounter = GetObject();
        if (counterObjectOnCounter.TryGetPlate(out PlateKitchenObject counterPlate))
        {
            if (counterPlate.State != PlateState.Assembly)
                return;

            var ingredients = counterPlate.GetKitchenObjectSoList();
            var menuItem = _menuItemResolver.TryResolveMenuItem(ingredients);
            if (menuItem == null)
            {
                // TODO(M5): replace with UI/audio feedback
                Debug.Log($"serve resolve failed");
                return;
            }
            if (!counterPlate.TryServe(menuItem))
            {
                // TODO(M5): replace with UI/audio feedback
                Debug.Log($"serve failed.");
            }
        }
    }
}