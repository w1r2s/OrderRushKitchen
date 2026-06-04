using Assets.Scripts.Managers.Input;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class OptionsUI : MonoBehaviour
    {
        public event EventHandler Opened;
        public event EventHandler Closed;
        public event EventHandler SfxVolumeRequested;
        public event EventHandler MusicVolumeRequested;
        public event EventHandler CloseRequested;
        public event EventHandler<OptionsRebindRequestedEventArgs> RebindRequested;

        [Header("Panels")]
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private GameObject dimmer;

        [Header("Common Buttons")]
        [SerializeField] private Button soundEffectsButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button closeButton;

        [Header("Rebind buttons")]
        [SerializeField] private Button moveUpButton;
        [SerializeField] private Button moveDownButton;
        [SerializeField] private Button moveLeftButton;
        [SerializeField] private Button moveRightButton;
        [SerializeField] private Button interactButton;
        [SerializeField] private Button altInteractButton;
        [SerializeField] private Button pauseButton;

        [Header("Common text")]
        [SerializeField] private TextMeshProUGUI soundEffectText;
        [SerializeField] private TextMeshProUGUI musicText;

        [Header("Rebind text")]
        [SerializeField] private TextMeshProUGUI moveUpText;
        [SerializeField] private TextMeshProUGUI moveDownText;
        [SerializeField] private TextMeshProUGUI moveLeftText;
        [SerializeField] private TextMeshProUGUI moveRightText;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private TextMeshProUGUI altInteractText;
        [SerializeField] private TextMeshProUGUI pauseText;

        [Header("Rebind dimmer")]
        [SerializeField] private Transform pressToRebindKeyTransform;

        public bool IsOpen => panelRoot.activeSelf;

        private void Awake()
        {
            soundEffectsButton.onClick.AddListener(OnSoundEffectsClicked);
            musicButton.onClick.AddListener(OnMusicClicked);
            closeButton.onClick.AddListener(OnCloseClicked);

            moveUpButton.onClick.AddListener(OnMoveUpClicked);
            moveDownButton.onClick.AddListener(OnMoveDownClicked);
            moveLeftButton.onClick.AddListener(OnMoveLeftClicked);
            moveRightButton.onClick.AddListener(OnMoveRightClicked);
            interactButton.onClick.AddListener(OnInteractClicked);
            altInteractButton.onClick.AddListener(OnAltInteractClicked);
            pauseButton.onClick.AddListener(OnPauseClicked);
        }

        private void OnDestroy()
        {
            soundEffectsButton.onClick.RemoveListener(OnSoundEffectsClicked);
            musicButton.onClick.RemoveListener(OnMusicClicked);
            closeButton.onClick.RemoveListener(OnCloseClicked);

            moveUpButton.onClick.RemoveListener(OnMoveUpClicked);
            moveDownButton.onClick.RemoveListener(OnMoveDownClicked);
            moveLeftButton.onClick.RemoveListener(OnMoveLeftClicked);
            moveRightButton.onClick.RemoveListener(OnMoveRightClicked);
            interactButton.onClick.RemoveListener(OnInteractClicked);
            altInteractButton.onClick.RemoveListener(OnAltInteractClicked);
            pauseButton.onClick.RemoveListener(OnPauseClicked);
        }

        public void InitializeHidden()
        {
            SetRebindOverlayVisible(false);
            panelRoot.SetActive(false);
            dimmer.SetActive(false);
        }

        public void Show()
        {
            var wasOpen = IsOpen;

            panelRoot.SetActive(true);
            dimmer.SetActive(true);
            soundEffectsButton.Select();

            if (!wasOpen)
                Opened?.Invoke(this, EventArgs.Empty);
        }

        public void Hide()
        {
            if (!IsOpen)
                return;

            panelRoot.SetActive(false);
            dimmer.SetActive(false);
            Closed?.Invoke(this, EventArgs.Empty);
        }

        public void SetAudioVolumes(float sfxVolume, float musicVolume)
        {
            soundEffectText.text = $"Sound Effects: {Mathf.Round(sfxVolume * 10f)}";
            musicText.text = $"Music: {Mathf.Round(musicVolume * 10f)}";
        }

        public void SetBindingText(InputKeyBinding binding, string text)
        {
            GetBindingText(binding).text = text;
        }

        public void SetRebindOverlayVisible(bool visible)
        {
            pressToRebindKeyTransform.gameObject.SetActive(visible);
        }

        private TextMeshProUGUI GetBindingText(InputKeyBinding binding)
        {
            return binding switch
            {
                InputKeyBinding.Move_Up => moveUpText,
                InputKeyBinding.Move_Down => moveDownText,
                InputKeyBinding.Move_Left => moveLeftText,
                InputKeyBinding.Move_Right => moveRightText,
                InputKeyBinding.Interact => interactText,
                InputKeyBinding.Alt_Interact => altInteractText,
                InputKeyBinding.Pause => pauseText,
                _ => throw new ArgumentOutOfRangeException(nameof(binding), binding, null)
            };
        }

        private void OnSoundEffectsClicked()
        {
            SfxVolumeRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnMusicClicked()
        {
            MusicVolumeRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnCloseClicked()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnMoveUpClicked() => RequestRebind(InputKeyBinding.Move_Up);
        private void OnMoveDownClicked() => RequestRebind(InputKeyBinding.Move_Down);
        private void OnMoveLeftClicked() => RequestRebind(InputKeyBinding.Move_Left);
        private void OnMoveRightClicked() => RequestRebind(InputKeyBinding.Move_Right);
        private void OnInteractClicked() => RequestRebind(InputKeyBinding.Interact);
        private void OnAltInteractClicked() => RequestRebind(InputKeyBinding.Alt_Interact);
        private void OnPauseClicked() => RequestRebind(InputKeyBinding.Pause);

        private void RequestRebind(InputKeyBinding binding)
        {
            RebindRequested?.Invoke(this, new OptionsRebindRequestedEventArgs(binding));
        }
    }

    public class OptionsRebindRequestedEventArgs : EventArgs
    {
        public InputKeyBinding Binding { get; }

        public OptionsRebindRequestedEventArgs(InputKeyBinding binding)
        {
            Binding = binding;
        }
    }
}