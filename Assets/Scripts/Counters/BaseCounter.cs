using Assets.Scripts.Managers.Sound;
using Zenject;

public abstract class BaseCounter : ObjectHolder
{
    private IAudioService _audioService;

    [Inject]
    protected void ConstructBase(IAudioService audioService)
    {
        _audioService = audioService;
    }

    public abstract void Interact(Player player);
    public virtual void InteractAlternate(Player player) { }

    protected void PlaceObjectFromPlayer(Player player)
    {
        var kitchenObject = player.RemoveObject();
        SetObject(kitchenObject);
        _audioService.PlayDrop(transform.position);
    }
}