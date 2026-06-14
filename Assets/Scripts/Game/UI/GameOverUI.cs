using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Game
{

    public class GameOverUI : MonoBehaviour
    {
        private IGameService _gameService;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI ordersDeliveredText;
        [SerializeField] private TextMeshProUGUI ordersDeliveredValueText;
        [SerializeField] private TextMeshProUGUI ordersFailedText;
        [SerializeField] private TextMeshProUGUI ordersFailedValueText;

        [Header("Actions")]
        [SerializeField] private Button retryButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button continueAfterAdButton;

        public event EventHandler RetryRequested;
        public event EventHandler MainMenuRequested;

        [Inject]
        private void Construct(IGameService gameService)
        {
            _gameService = gameService;
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
            RetryRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ReturnToMainMenu()
        {
            MainMenuRequested?.Invoke(this, EventArgs.Empty);
        }
    }

}
