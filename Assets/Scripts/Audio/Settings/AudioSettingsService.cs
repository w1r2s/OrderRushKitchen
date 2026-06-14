using System;
using UnityEngine;

namespace OrderRushKitchen.Audio
{
    public class AudioSettingsService : IAudioSettingsService
    {
        private const float Step = 0.1f;

        private readonly IAudioSettingsStorage _storage;

        public float SfxVolume { get; private set; }
        public float MusicVolume { get; private set; }

        public event EventHandler OnSettingsChanged;

        public AudioSettingsService(IAudioSettingsStorage storage)
        {
            _storage = storage;
            SfxVolume = Mathf.Clamp01(_storage.LoadVolume(AudioVolumeChannel.Sfx));
            MusicVolume = Mathf.Clamp01(_storage.LoadVolume(AudioVolumeChannel.Music));
        }

        public void StepSfxVolume()
        {
            SfxVolume = GetNextVolume(SfxVolume);
            _storage.SaveVolume(AudioVolumeChannel.Sfx, SfxVolume);
            OnSettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void StepMusicVolume()
        {
            MusicVolume = GetNextVolume(MusicVolume);
            _storage.SaveVolume(AudioVolumeChannel.Music, MusicVolume);
            OnSettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private static float GetNextVolume(float current)
        {
            var next = Mathf.Round((current + Step) * 10f) / 10f;
            return next > 1f ? 0f : next;
        }
    }
}