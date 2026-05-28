using Zenject;

namespace Assets.Scripts.Audio
{
    public class MainMenuMusicStarter : IInitializable
    {
        private readonly IMusicService _musicService;

        public MainMenuMusicStarter(IMusicService musicService)
        {
            _musicService = musicService;
        }

        public void Initialize()
        {
            _musicService.Play(MusicTrackId.MainMenu);
        }
    }
}