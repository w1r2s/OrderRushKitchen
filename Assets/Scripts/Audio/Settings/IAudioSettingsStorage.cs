namespace OrderRushKitchen.Audio
{
    public enum AudioVolumeChannel
    {
        Sfx,
        Music
    }

    public interface IAudioSettingsStorage
    {
        float LoadVolume(AudioVolumeChannel channel);
        void SaveVolume(AudioVolumeChannel channel, float volume);
    }
}
