using Assets.Scripts.Audio;
using Assets.Scripts.Managers.Input;
using System;
using Zenject;

namespace Assets.Scripts.UI
{
    public class OptionsUIController : IInitializable, IDisposable
    {
        private readonly OptionsUI _optionsUI;
        private readonly IAudioSettingsService _audioSettings;
        private readonly IInputRebindingService _inputRebindingService;

        private bool _isRebinding;

        public OptionsUIController(OptionsUI optionsUI, IAudioSettingsService audioSettings, IInputRebindingService inputRebindingService)
        {
            _optionsUI = optionsUI;
            _audioSettings = audioSettings;
            _inputRebindingService = inputRebindingService;
        }

        public void Initialize()
        {
            _optionsUI.Opened += OptionsUI_Opened;
            _optionsUI.SfxVolumeRequested += OptionsUI_SfxVolumeRequested;
            _optionsUI.MusicVolumeRequested += OptionsUI_MusicVolumeRequested;
            _optionsUI.CloseRequested += OptionsUI_CloseRequested;
            _optionsUI.RebindRequested += OptionsUI_RebindRequested;

            _audioSettings.OnSettingsChanged += AudioSettings_OnSettingsChanged;

            RefreshView();
            _optionsUI.InitializeHidden();
        }

        public void Dispose()
        {
            _optionsUI.Opened -= OptionsUI_Opened;
            _optionsUI.SfxVolumeRequested -= OptionsUI_SfxVolumeRequested;
            _optionsUI.MusicVolumeRequested -= OptionsUI_MusicVolumeRequested;
            _optionsUI.CloseRequested -= OptionsUI_CloseRequested;
            _optionsUI.RebindRequested -= OptionsUI_RebindRequested;

            _audioSettings.OnSettingsChanged -= AudioSettings_OnSettingsChanged;
        }

        private void OptionsUI_Opened(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void OptionsUI_SfxVolumeRequested(object sender, EventArgs e)
        {
            _audioSettings.StepSfxVolume();
            RefreshAudioView();
        }

        private void OptionsUI_MusicVolumeRequested(object sender, EventArgs e)
        {
            _audioSettings.StepMusicVolume();
            RefreshAudioView();
        }

        private void OptionsUI_CloseRequested(object sender, EventArgs e)
        {
            _optionsUI.Hide();
        }

        private void OptionsUI_RebindRequested(object sender, OptionsRebindRequestedEventArgs e)
        {
            if (_isRebinding)
                return;

            _isRebinding = true;
            _optionsUI.SetRebindOverlayVisible(true);

            _inputRebindingService.RebindKeyBinding(e.Binding, completed =>
            {
                _isRebinding = false;
                _optionsUI.SetRebindOverlayVisible(false);

                if (completed)
                    RefreshBindingView(e.Binding);
            });
        }

        private void AudioSettings_OnSettingsChanged(object sender, EventArgs e)
        {
            RefreshAudioView();
        }

        private void RefreshView()
        {
            RefreshAudioView();
            RefreshBindingsView();
        }

        private void RefreshAudioView()
        {
            _optionsUI.SetAudioVolumes(_audioSettings.SfxVolume, _audioSettings.MusicVolume);
        }

        private void RefreshBindingsView()
        {
            RefreshBindingView(InputKeyBinding.Move_Up);
            RefreshBindingView(InputKeyBinding.Move_Down);
            RefreshBindingView(InputKeyBinding.Move_Left);
            RefreshBindingView(InputKeyBinding.Move_Right);
            RefreshBindingView(InputKeyBinding.Interact);
            RefreshBindingView(InputKeyBinding.Alt_Interact);
            RefreshBindingView(InputKeyBinding.Pause);
        }

        private void RefreshBindingView(InputKeyBinding binding)
        {
            _optionsUI.SetBindingText(binding, _inputRebindingService.GetKeyBindingText(binding));
        }
    }
}
