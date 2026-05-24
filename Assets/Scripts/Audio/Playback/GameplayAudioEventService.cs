using UnityEngine;
using Zenject;

namespace Assets.Scripts.Audio
{
    public class GameplayAudioEventService : IGameplayAudioEventService
    {
        private readonly GameplayAudioLibrarySo _audioLibrarySo;
        private readonly IAudioSettingsService _audioSettingsService;
        private readonly IOneShotAudioPlayer _oneShotAudioPlayer;

        [Inject]
        public GameplayAudioEventService(GameplayAudioLibrarySo audioLibrarySo, IAudioSettingsService audioSettingsService, IOneShotAudioPlayer oneShotAudioPlayer)
        {
            _audioLibrarySo = audioLibrarySo;
            _audioSettingsService = audioSettingsService;
            _oneShotAudioPlayer = oneShotAudioPlayer;
        }
        public void Play(GameplayAudioEvent audioEvent, Vector3 position, float volumeMultiplier = 1f)
        {
            if (!_audioLibrarySo.TryGetEventClips(audioEvent, out var clips))
                return;

            if (clips == null || clips.Count == 0)
                return;

            var volume = Mathf.Clamp01(_audioSettingsService.SfxVolume * volumeMultiplier);
            var clip = clips[Random.Range(0, clips.Count)];

            if (clip == null)
                return;

            _oneShotAudioPlayer.Play(clip, position, volume);
        }
    }
}
