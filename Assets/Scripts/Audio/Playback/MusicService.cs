using UnityEngine;
using Zenject;

namespace Assets.Scripts.Audio
{
    public class MusicService : IMusicService
    {
        private readonly MusicTrackLibrarySo _trackLibrary;
        private readonly IMusicPlayer _musicPlayer;

        private MusicTrackId? _currentTrackId;

        [Inject]
        public MusicService(MusicTrackLibrarySo trackLibrary, IMusicPlayer musicPlayer)
        {
            _trackLibrary = trackLibrary;
            _musicPlayer = musicPlayer;
        }

        public void Play(MusicTrackId trackId)
        {
            if (_currentTrackId == trackId)
                return;

            if (!_trackLibrary.TryGetClip(trackId, out var clip))
            {
                Debug.LogError($"Music clip for track '{trackId}' was not found.");
                return;
            }

            _musicPlayer.Play(clip);
            _currentTrackId = trackId;
        }

        public void Stop()
        {
            _musicPlayer.Stop();
            _currentTrackId = null;
        }
    }
}