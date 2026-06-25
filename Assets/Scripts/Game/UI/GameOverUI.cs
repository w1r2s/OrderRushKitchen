using OrderRushKitchen.Level;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace OrderRushKitchen.Game
{

    public class GameOverUI : MonoBehaviour
    {
        private IGameService _gameService;
        private ILevelProgressionService _levelProgressionService;

        [Header("Panels")]
        [SerializeField] private Transform dimmer;
        [SerializeField] private Transform panel;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI ordersDeliveredValueText;
        [SerializeField] private TextMeshProUGUI ordersFailedValueText;

        [Header("Actions")]
        [SerializeField] private Button retryButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button continueAfterAdButton;

        public event EventHandler RetryRequested;
        public event EventHandler MainMenuRequested;

        [Inject]
        private void Construct(IGameService gameService, ILevelProgressionService levelProgressionService)
        {
            _gameService = gameService;
            _levelProgressionService = levelProgressionService;
        }

        private void Start()
        {
            if (_gameService == null || _levelProgressionService == null)
                return;

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
            if (ordersDeliveredValueText != null)
                ordersDeliveredValueText.text = _levelProgressionService.CompletedOrdersInLevel.ToString();

            if (ordersFailedValueText != null)
                ordersFailedValueText.text = _levelProgressionService.FailedOrdersInLevel.ToString();
        }

        private void Show()
        {
            dimmer.gameObject.SetActive(true);
            panel.gameObject.SetActive(true);
        }

        private void Hide()
        {
            dimmer.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
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
