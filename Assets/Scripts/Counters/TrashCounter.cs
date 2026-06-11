using Assets.Scripts.Audio;
using Zenject;

public class TrashCounter : BaseCounter
{
    private IGameplayAudioEventService _audioService;

    [Inject]
    private void Construct(IGameplayAudioEventService audioService)
    {
        _audioService = audioService;
    }
    public override void Interact(Player player)
    {
        if (!player.TryRemoveAndDestroyObject())
            return;

        _audioService.Play(GameplayAudioEvent.TrashItem, transform.position);
    }
}
