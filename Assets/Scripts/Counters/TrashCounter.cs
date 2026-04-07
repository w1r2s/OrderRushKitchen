using Assets.Scripts.Managers.Sound;
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
        if (player.HasObject)
        {
            var obj = player.GetObject();
            player.RemoveObject();
            Destroy(obj.gameObject);

            _audioService.PlayTrash(transform.position);
        }
    }
}
