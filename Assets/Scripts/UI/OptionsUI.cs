using Assets.Scripts.Audio;
using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OptionsUI : MonoBehaviour
{
    public event EventHandler Opened;
    public event EventHandler Closed;

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button altInteractButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI altInteractText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private Transform pressToRebindKeyTransform;

    private Action onCloseButtonAction;
    private IGamePauseService _pauseService;
    private IInputRebindingService _inputRebindingService;
    private IAudioSettingsService _audioSettings;

    [Inject]
    private void Construct(IGamePauseService pauseService, IInputRebindingService inputService, IAudioSettingsService audioSettings)
    {
        _pauseService = pauseService;
        _inputRebindingService = inputService;
        _audioSettings = audioSettings;
    }
    private void Awake()
    {

        soundEffectsButton.onClick.AddListener(() =>
        {
            _audioSettings.StepSfxVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            _audioSettings.StepMusicVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(InputKeyBinding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(InputKeyBinding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(InputKeyBinding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(InputKeyBinding.Move_Right);
        });
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(InputKeyBinding.Interact);
        });
        altInteractButton.onClick.AddListener(() =>
        {
            RebindBinding(InputKeyBinding.Alt_Interact);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(InputKeyBinding.Pause);
        });
    }
    private void Start()
    {
        _pauseService.OnPauseChanged += PauseService_OnPauseChanged;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }
    private void OnDestroy()
    {
        if (_pauseService != null)
            _pauseService.OnPauseChanged -= PauseService_OnPauseChanged;
    }
    private void PauseService_OnPauseChanged(object sender, EventArgs e)
    {
        if (!_pauseService.HasPause(GamePauseReason.UserPause))
            Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = $"Sound Effects: {Mathf.Round(_audioSettings.SfxVolume * 10f)}";
        musicText.text = "Music: " + Mathf.Round(_audioSettings.MusicVolume * 10f);

        moveUpText.text = _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Up);
        moveDownText.text = _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Down);
        moveLeftText.text = _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Left);
        moveRightText.text = _inputRebindingService.GetKeyBindingText(InputKeyBinding.Move_Right);
        interactText.text = _inputRebindingService.GetKeyBindingText(InputKeyBinding.Interact);
        altInteractText.text = _inputRebindingService.GetKeyBindingText(InputKeyBinding.Alt_Interact);
        pauseText.text = _inputRebindingService.GetKeyBindingText(InputKeyBinding.Pause);
    }
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        var wasOpen = gameObject.activeSelf;

        gameObject.SetActive(true);
        soundEffectsButton.Select();

        if (!wasOpen)
            Opened?.Invoke(this, EventArgs.Empty);
    }
    private void Hide()
    {
        if (!gameObject.activeSelf)
            return;

        gameObject.SetActive(false);
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
    private void RebindBinding(InputKeyBinding binding)
    {
        ShowPressToRebindKey();

        _inputRebindingService.RebindKeyBinding(binding, completed =>
        {
            HidePressToRebindKey();

            if (completed)
                UpdateVisual();
        });
    }
}
