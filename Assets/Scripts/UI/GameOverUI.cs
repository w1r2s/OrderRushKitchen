using Assets.Scripts.Managers.Game;
using TMPro;
using UnityEngine;
using Zenject;

public class GameOverUI : MonoBehaviour
{
    private IDeliveryService _deliveryService;
    private IGameService _gameService;

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    
    
    [Inject]
    private void Construct(IGameService gameService,IDeliveryService deliveryService)
    {
        _gameService = gameService;
        _deliveryService = deliveryService;
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
            recipesDeliveredText.text = _deliveryService.GetSuccessfulRecipesAmount().ToString();
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
