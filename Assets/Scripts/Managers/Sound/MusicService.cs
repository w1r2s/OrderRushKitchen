namespace Assets.Scripts.Managers.Sound
{
    internal class MusicService : IMusicService
    {
        private IMusicStorage _musicStorage;
        private MusicManager _musicManager;

        float volume = 0.3f;
        public MusicService(IMusicStorage musicStorage, MusicManager musicManager)
        {
            _musicStorage = musicStorage;
            _musicManager = musicManager;

            volume = _musicStorage.LoadVolume();
        }
        public void Initialize()
        {
            _musicManager.SetVolume(volume);
        }
        public void ChangeVolume()
        {
            volume += .1f;
            if (volume > 1.1f)
            {
                volume = 0;
            }
            _musicManager.SetVolume(volume);
            _musicStorage.SaveVolume(volume);
        }

        public float GetVolume()
        {
            return volume;
        }
    }
}
