using Assets.Scripts.Managers.Game;
using Assets.Scripts.Navigation;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverUI : MonoBehaviour
{
    private IGameService _gameService;
    private INavigationService _navigationService;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI ordersDeliveredText;
    [SerializeField] private TextMeshProUGUI ordersDeliveredValueText;
    [SerializeField] private TextMeshProUGUI ordersFailedText;
    [SerializeField] private TextMeshProUGUI ordersFailedValueText;

    [Header("Actions")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button continueAfterAdButton;

    [Inject]
    private void Construct(IGameService gameService, INavigationService navigationService)
    {
        _gameService = gameService;
        _navigationService = navigationService;
    }

    private void Start()
    {
        _gameService.OnGameStateChanged += GameService_OnGameStateChanged;

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(Retry);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }

        if (continueAfterAdButton != null)
        {
            continueAfterAdButton.interactable = false;
        }

        Hide();
    }

    private void OnDestroy()
    {
        if (_gameService != null)
        {
            _gameService.OnGameStateChanged -= GameService_OnGameStateChanged;
        }

        if (retryButton != null)
        {
            retryButton.onClick.RemoveListener(Retry);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(ReturnToMainMenu);
        }
    }

    private void GameService_OnGameStateChanged(object sender, EventArgs e)
    {
        if (_gameService.IsGameOver())
        {
            RefreshStats();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void RefreshStats()
    {
        if (ordersDeliveredText != null)
        {
            ordersDeliveredText.text = "ORDERS DELIVERED";
        }

        if (ordersDeliveredValueText != null)
        {
            ordersDeliveredValueText.text = "--";
        }

        if (ordersFailedText != null)
        {
            ordersFailedText.text = "ORDERS FAILED";
        }

        if (ordersFailedValueText != null)
        {
            ordersFailedValueText.text = "--";
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

    private void Retry()
    {
        _navigationService.ReloadGameAsync(CancellationToken.None).Forget(Debug.LogException);
    }

    private void ReturnToMainMenu()
    {
        _navigationService.LoadMainMenuAsync(CancellationToken.None).Forget(Debug.LogException);
    }
}
