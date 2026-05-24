using Assets.Scripts.Audio;
using Assets.Scripts.Managers.Game;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Player))]
public class PlayerSounds : MonoBehaviour
{
    private Player _player;
    private IGameplayAudioEventService _audioService;
    private IGameClock _gameClock;

    private float footstepTimer;
    [SerializeField] private float footstepTimerMax = 0.3f;
    [SerializeField] private float footstepVolumeMultiplier = 0.5f;

    [Inject]
    private void Construct(IGameplayAudioEventService audioService, IGameClock gameClock)
    {
        _audioService = audioService;
        _gameClock = gameClock;
    }
    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void Update()
    {
        if (!_player.IsWalking())
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer -= _gameClock.DeltaTime;
        if (footstepTimer > 0f)
            return;

        footstepTimer = footstepTimerMax;
        _audioService.Play(GameplayAudioEvent.PlayerFootstep, transform.position, footstepVolumeMultiplier);
    }
}