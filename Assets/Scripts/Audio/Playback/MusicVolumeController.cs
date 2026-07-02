using System;
using Zenject;

namespace OrderRushKitchen.Audio
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
            ApplyVolume();
            _audioSettingsService.OnSettingsChanged += AudioSettingsService_OnSettingsChanged;
        }

        private void AudioSettingsService_OnSettingsChanged(object sender, EventArgs e)
        {
            ApplyVolume();
        }

        public void Dispose()
        {
            _audioSettingsService.OnSettingsChanged -= AudioSettingsService_OnSettingsChanged;
        }

        private void ApplyVolume()
        {
            _musicPlayer.SetVolume(AudioVolumeMapper.ToMusicPlaybackVolume(_audioSettingsService.MusicVolume));
        }
    }
}
