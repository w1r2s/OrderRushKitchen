using Assets.Scripts.Managers.Game;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;


    private OptionsUI _optionsUI;
    private IGamePauseService _pauseService;

    [Inject]
    private void Construct(IGamePauseService pauseService, OptionsUI optionsUI)
    {
        _pauseService = pauseService;
        _optionsUI = optionsUI;
    }
    private void Start()
    {
        _pauseService.OnPauseChanged += PauseService_OnPauseChanged;
        resumeButton.onClick.AddListener(() =>
        {
            _pauseService.RemovePause(GamePauseReason.UserPause);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            LoadingManager.Load(LoadingManager.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            _optionsUI.Show(Show);
        });
        RefreshVisibility();
    }
    private void OnDestroy()
    {
        if (_pauseService != null)
        {
            _pauseService.OnPauseChanged -= PauseService_OnPauseChanged;
        }
    }
    private void PauseService_OnPauseChanged(object sender, EventArgs e)
    {
        RefreshVisibility();
    }

    private void RefreshVisibility()
    {
        if (_pauseService.HasPause(GamePauseReason.UserPause))
            Show();
        else
            Hide();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
