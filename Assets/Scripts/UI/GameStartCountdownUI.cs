using Assets.Scripts.Managers.Game;
using TMPro;
using UnityEngine;
using Zenject;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountdownText;
    private IGameService _gameService;

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
        if (_gameService.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
        CountdownText.text = _gameService.GetCountdownToStartTimer().ToString("#");
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
