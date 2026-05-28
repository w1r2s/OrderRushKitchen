using Zenject;

namespace Assets.Scripts.Audio
{
    public class GameplayMusicStarter : IInitializable
    {
        private readonly IMusicService _musicService;

        public GameplayMusicStarter(IMusicService musicService)
        {
            _musicService = musicService;
        }

        public void Initialize()
        {
            _musicService.Play(MusicTrackId.Gameplay);
        }
    }
}
