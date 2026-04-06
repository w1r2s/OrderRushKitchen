using Assets.Scripts.Managers.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private IGameService _gameService;

    [Inject]
    private void Construct(IGameService gameService)
    {
        _gameService = gameService;
    }
    private void Start()
    {
        _gameService.OnGamePaused += GameManager_OnGamePaused;
        _gameService.OnGameUnpaused += GameManager_OnGameUnpaused;
        resumeButton.onClick.AddListener(() =>
        {
            _gameService.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            LoadingManager.Load(LoadingManager.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            //TODO: ”брать OptionsUI в DI
            OptionsUI.Instance.Show(Show);
        });
        Hide();
    }
    private void OnDestroy()
    {
        if (_gameService != null)
        {
            _gameService.OnGamePaused -= GameManager_OnGamePaused;
            _gameService.OnGameUnpaused -= GameManager_OnGameUnpaused;
        }
    }
    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
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
