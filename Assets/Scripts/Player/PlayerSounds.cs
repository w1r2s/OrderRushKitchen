using Assets.Scripts.Managers.Sound;
using UnityEngine;
using Zenject;

public class PlayerSounds : MonoBehaviour
{
    private Player _player;
    private IAudioService _audioService;

    private float footstepTimer;
    [SerializeField] private float footstepTimerMax = 0.3f;

    [Inject]
    private void Construct(Player player, IAudioService audioService)
    {
        _player = player;
        _audioService = audioService;
    }
    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;
            if (_player.IsWalking())
            {
                float volume = 1;
                _audioService.PlayFootstep(_player.transform.position, volume);
            }
        }
    }

}
