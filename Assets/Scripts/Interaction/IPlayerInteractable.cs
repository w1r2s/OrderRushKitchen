namespace Assets.Scripts.Interaction
{
    public interface IPlayerInteractable
    {
        void Interact(Player player);
        bool TryInteractAlternate(Player player);
    }
}
