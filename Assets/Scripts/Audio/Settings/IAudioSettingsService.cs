using System;

namespace OrderRushKitchen.Audio
{
    public interface IAudioSettingsService
    {
        float SfxVolume { get; }
        float MusicVolume { get; }

        event EventHandler OnSettingsChanged;

        void StepSfxVolume();
        void StepMusicVolume();
    }
}
