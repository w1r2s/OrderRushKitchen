using Zenject;

namespace Assets.Scripts.Managers.Sound
{
    public class MusicInitializer : IInitializable
    {
        private readonly IMusicService _musicService;
        public MusicInitializer(IMusicService musicService)
        {
            _musicService = musicService;
        }
        public void Initialize()
        {
            _musicService.Initialize();
        }

    }
}
