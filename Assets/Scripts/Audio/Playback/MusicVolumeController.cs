using System;
using Zenject;

namespace Assets.Scripts.Audio
{
    public class MusicVolumeController : IInitializable, IDisposable
    {
        private readonly IAudioSettingsService _audioSettingsService;
        private readonly IMusicPlayer _musicPlayer;

        [Inject]
        public MusicVolumeController(IAudioSettingsService audioSettingsService, IMusicPlayer musicPlayer)
        {
            _audioSettingsService = audioSettingsService;
            _musicPlayer = musicPlayer;
        }

  

        public void Initialize()
        {
            _musicPlayer.SetVolume(_audioSettingsService.MusicVolume);
            _audioSettingsService.OnSettingsChanged += AudioSettingsService_OnSettingsChanged;
        }

        private void AudioSettingsService_OnSettingsChanged(object sender, EventArgs e)
        {
            _musicPlayer.SetVolume(_audioSettingsService.MusicVolume);
        }

        public void Dispose()
        {
            _audioSettingsService.OnSettingsChanged -= AudioSettingsService_OnSettingsChanged;
        }
    }
}
