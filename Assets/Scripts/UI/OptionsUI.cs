using Assets.Scripts.Managers.Game;
using Assets.Scripts.Managers.Input;
using Assets.Scripts.Managers.Sound;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

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
    private IGameService _gameService;
    private IInputService _inputService;
    private IAudioService _audioService;
    private IMusicService _musicService;

    [Inject]
    private void Construct(IGameService gameService, IInputService inputService, IAudioService audioService,IMusicService musicService)
    {
        _gameService = gameService;
        _inputService = inputService;
        _audioService = audioService;
        _musicService = musicService;
    }
    private void Awake()
    {

        soundEffectsButton.onClick.AddListener(() =>
        {
            _audioService.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            _musicService.ChangeVolume();
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
        _gameService.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = $"Sound Effects: {Mathf.Round(_audioService.GetVolume() * 10f)}";
        musicText.text = "Music: " + Mathf.Round(_musicService.GetVolume() * 10f);

        moveUpText.text = _inputService.GetKeyBindingText(InputKeyBinding.Move_Up);
        moveDownText.text = _inputService.GetKeyBindingText(InputKeyBinding.Move_Down);
        moveLeftText.text = _inputService.GetKeyBindingText(InputKeyBinding.Move_Left);
        moveRightText.text = _inputService.GetKeyBindingText(InputKeyBinding.Move_Right);
        interactText.text = _inputService.GetKeyBindingText(InputKeyBinding.Interact);
        altInteractText.text = _inputService.GetKeyBindingText(InputKeyBinding.Alt_Interact);
        pauseText.text = _inputService.GetKeyBindingText(InputKeyBinding.Pause);
    }
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;


        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
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
        _inputService.RebindKeyBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
