using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Audio
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
            if (TryResolveClip(audioEvent, volumeMultiplier, out var clip, out var volume))
            {
                _oneShotAudioPlayer.Play(clip, position, volume);
            }
        }

        public void PlayGlobal(GameplayAudioEvent audioEvent, float volumeMultiplier = 1)
        {
            if (TryResolveClip(audioEvent, volumeMultiplier, out var clip, out var volume))
            {
                _oneShotAudioPlayer.PlayGlobal(clip, volume);
            }
        }

        private bool TryResolveClip(GameplayAudioEvent audioEvent, float volumeMultiplier, out AudioClip clip, out float volume)
        {
            clip = null;
            volume = 0f;

            if (!_audioLibrarySo.TryGetEventClips(audioEvent, out var clips))
                return false;

            if (clips == null || clips.Count == 0)
                return false;

            volume = Mathf.Clamp01(AudioVolumeMapper.ToSfxPlaybackVolume(_audioSettingsService.SfxVolume) * volumeMultiplier);
            clip = clips[Random.Range(0, clips.Count)];

            if (clip == null)
                return false;

            return true;

        }
    }
}
