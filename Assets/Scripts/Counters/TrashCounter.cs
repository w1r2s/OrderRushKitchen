using Assets.Scripts.Managers.Sound;
using System;
using Zenject;

public class TrashCounter : BaseCounter
{
    private IAudioService _audioService;

    [Inject]
    private void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            _audioService.PlayTrash(transform.position);
        }
    }
}
