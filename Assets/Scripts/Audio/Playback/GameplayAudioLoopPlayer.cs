using OrderRushKitchen.Game;
using System;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Audio
{
    public class GameplayAudioLoopPlayer : MonoBehaviour
    {
        [SerializeField] private GameplayAudioLoop loopType;
        [SerializeField] private AudioSource audioSource;
        [SerializeField, Range(0f, 1f)] private float volumeMultiplier = 1f;

        private GameplayAudioLibrarySo _audioLibrary;
        private IAudioSettingsService _audioSettings;
        private IGamePauseService _pauseService;

        private bool _requestedPlaying;

        [Inject]
        private void Construct(GameplayAudioLibrarySo audioLibrary, IAudioSettingsService audioSettings, IGamePauseService pauseService)
        {
            _audioLibrary = audioLibrary;
            _audioSettings = audioSettings;
            _pauseService = pauseService;
        }

        private void Awake()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            if (audioSource == null)
            {
                Debug.LogError($"{nameof(GameplayAudioLoopPlayer)} requires {nameof(AudioSource)}.", this);
                enabled = false;
                return;
            }
            audioSource.playOnAwake = false;
            audioSource.loop = true;
        }

        private void Start()
        {
            if (!_audioLibrary.TryGetLoopClip(loopType, out var clip))
            {
                Debug.LogError($"{nameof(GameplayAudioLoopPlayer)} couldn't find clip");
                audioSource.enabled = false;
                return;
            }
            audioSource.clip = clip;
            audioSource.volume = Mathf.Clamp01(_audioSettings.SfxVolume * volumeMultiplier);

            _audioSettings.OnSettingsChanged += AudioSettings_OnSettingsChanged;
            _pauseService.OnPauseChanged += PauseService_OnPauseChanged;

            RefreshPlayback();
        }

        private void OnDestroy()
        {
            if (_audioSettings != null)
                _audioSettings.OnSettingsChanged -= AudioSettings_OnSettingsChanged;

            if (_pauseService != null)
                _pauseService.OnPauseChanged -= PauseService_OnPauseChanged;
        }

        public void SetPlaying(bool playing)
        {
            _requestedPlaying = playing;
            RefreshPlayback();
        }

        private void RefreshPlayback()
        {
            if (audioSource == null || audioSource.clip == null)
                return;

            ApplyVolume();

            var shouldPlay = _requestedPlaying && !_pauseService.IsPaused;

            if (shouldPlay)
            {
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying)
                    audioSource.Pause();
            }
        }
        private void ApplyVolume()
        {
            audioSource.volume = Mathf.Clamp01(_audioSettings.SfxVolume * volumeMultiplier);
        }

        private void AudioSettings_OnSettingsChanged(object sender, EventArgs e)
        {
            RefreshPlayback();
        }
        private void PauseService_OnPauseChanged(object sender, EventArgs e)
        {
            RefreshPlayback();
        }
    }
}