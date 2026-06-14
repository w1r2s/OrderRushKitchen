using System;
using UnityEngine;

namespace OrderRushKitchen.Audio
{
    public class PlayerPrefsAudioSettingsStorage : IAudioSettingsStorage
    {
        private const string SfxVolumeKey = "PlayerSoundEffectsVolume";
        private const string MusicVolumeKey = "PlayerMusicVolume";
        public float LoadVolume(AudioVolumeChannel channel)
        {
            return channel switch
            {
                AudioVolumeChannel.Sfx => PlayerPrefs.GetFloat(SfxVolumeKey, 1f),
                AudioVolumeChannel.Music => PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f),
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }

        public void SaveVolume(AudioVolumeChannel channel, float volume)
        {
            switch (channel)
            {
                case AudioVolumeChannel.Sfx:
                    {
                        var clampedVolume = Mathf.Clamp01(volume);
                        PlayerPrefs.SetFloat(SfxVolumeKey, clampedVolume);
                        PlayerPrefs.Save();
                        break;
                    }
                case AudioVolumeChannel.Music:
                    {
                        var clampedVolume = Mathf.Clamp01(volume);
                        PlayerPrefs.SetFloat(MusicVolumeKey, clampedVolume);
                        PlayerPrefs.Save();
                        break;
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
                    }
            }
        }
    }
}
