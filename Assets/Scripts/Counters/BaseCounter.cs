public abstract class BaseCounter : ObjectHolder
{
    public abstract void Interact(Player player);
    public virtual void InteractAlternate(Player player) { }
}