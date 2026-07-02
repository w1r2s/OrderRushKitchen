using UnityEngine;

namespace OrderRushKitchen.Audio
{
    public static class AudioVolumeMapper
    {
        private const float MusicMaxVolume = 0.3f;

        public static float ToSfxPlaybackVolume(float settingsVolume)
        {
            return Mathf.Sqrt(Mathf.Clamp01(settingsVolume));
        }

        public static float ToMusicPlaybackVolume(float settingsVolume)
        {
            return Mathf.Clamp01(settingsVolume) * MusicMaxVolume;
        }
    }
}
