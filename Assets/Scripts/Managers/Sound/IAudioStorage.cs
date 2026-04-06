namespace Assets.Scripts.Managers.Sound
{
    public interface IAudioStorage
    {
        float LoadVolume();
        void SaveVolume(float volume);
    }
}
