using Assets.Scripts.Managers.Game;
using Assets.Scripts.Navigation;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
    private INavigationService _navigationService;

    [Inject]
    private void Construct(IGamePauseService pauseService, OptionsUI optionsUI, INavigationService navigationService)
    {
        _pauseService = pauseService;
        _optionsUI = optionsUI;
        _navigationService = navigationService;
    }
    private void Start()
    {
        _pauseService.OnPauseChanged += PauseService_OnPauseChanged;

        resumeButton.onClick.AddListener(Resume);

        mainMenuButton.onClick.AddListener(ReturnToMenu);

        optionsButton.onClick.AddListener(OpenOptions);

        RefreshVisibility();
    }
    private void OnDestroy()
    {
        if (_pauseService != null)
        {
            _pauseService.OnPauseChanged -= PauseService_OnPauseChanged;
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(ReturnToMenu);
        }
        if (optionsButton != null)
        {
            optionsButton.onClick.RemoveListener(OpenOptions);
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveListener(Resume);
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
    private void Resume()
    {
        _pauseService.RemovePause(GamePauseReason.UserPause);
    }

    private void OpenOptions()
    {
        Hide();
        _optionsUI.Show(Show);
    }
    private void ReturnToMenu()
    {
        _navigationService.LoadMainMenuAsync(CancellationToken.None).Forget(Debug.LogException);
    }
}