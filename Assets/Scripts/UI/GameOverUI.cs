using Assets.Scripts.Managers.Game;
using TMPro;
using UnityEngine;
using Zenject;

public class GameOverUI : MonoBehaviour
{
    private IGameService _gameService;

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;


    [Inject]
    private void Construct(IGameService gameService)
    {
        _gameService = gameService;
    }
    private void Start()
    {
        _gameService.OnGameStateChanged += GameManager_OnGameStateChanged;
        Hide();
    }
    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (_gameService.IsGameOver())
        {
             recipesDeliveredText.text = "In progress";
            Show();
        }
        else
        {
            Hide();
        }
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
