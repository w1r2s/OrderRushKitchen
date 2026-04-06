namespace Assets.Scripts.Managers.Sound
{
    public interface IMusicService
    {
        void ChangeVolume();
        float GetVolume();

        void Initialize();
    }
}
