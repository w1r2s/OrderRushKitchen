using Assets.Scripts.Audio;
using Assets.Scripts.Interaction;
using Zenject;

public abstract class BaseCounter : ObjectHolder, IPlayerInteractable
{
    private IGameplayAudioEventService _audioService;

    [Inject]
    protected void ConstructBase(IGameplayAudioEventService audioService)
    {
        _audioService = audioService;
    }

    public abstract void Interact(Player player);
    public virtual void InteractAlternate(Player player) { }

    protected void PlaceObjectFromPlayer(Player player)
    {
        var kitchenObject = player.RemoveObject();
        SetObject(kitchenObject);
        _audioService.Play(GameplayAudioEvent.DropGeneric,transform.position);
    }
}