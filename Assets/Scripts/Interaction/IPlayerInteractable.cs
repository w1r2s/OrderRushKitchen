using OrderRushKitchen.PlayerControl;

namespace OrderRushKitchen.Interaction
{
    public interface IPlayerInteractable
    {
        void Interact(Player player);
        bool TryInteractAlternate(Player player);
    }
}
