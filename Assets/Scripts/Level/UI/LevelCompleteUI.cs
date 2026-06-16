using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Level
{
    public class LevelCompleteUI : MonoBehaviour
    {
        private ILevelCompletionFlowService _levelFlowService;

        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject dimmer;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button nextButton;


        [Inject]
        private void Construct(ILevelCompletionFlowService levelFlowService)
        {
            _levelFlowService = levelFlowService;
        }

        private void Start()
        {
            if (_levelFlowService == null)
                return;

            if (panel == null || dimmer == null || nextButton == null || titleText == null || statusText == null)
                return;

            _levelFlowService.OnCompletionShown += LevelFlowService_OnCompletionShown;
            _levelFlowService.OnCompletionHidden += LevelFlowService_OnCompletionHidden;

            nextButton.onClick.AddListener(Next);

            if (menuButton != null)
                menuButton.onClick.AddListener(Menu);

            Hide();
        }

        private void OnDestroy()
        {
            if (_levelFlowService != null)
            {
                _levelFlowService.OnCompletionShown -= LevelFlowService_OnCompletionShown;
                _levelFlowService.OnCompletionHidden -= LevelFlowService_OnCompletionHidden;
            }

            if (nextButton != null)
                nextButton.onClick.RemoveListener(Next);

            if (menuButton != null)
                menuButton.onClick.RemoveListener(Menu);
        }

        private void LevelFlowService_OnCompletionShown(object sender, LevelCompletionShownEventArgs e)
        {
            titleText.text = $"Level {e.LevelIndex} completed";
            statusText.text = $"Orders: {e.CompletedOrders} / {e.RequiredOrders}";
            Show();
        }

        private void LevelFlowService_OnCompletionHidden(object sender, EventArgs e)
        {
            Hide();
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

        private void Next()
        {
            _levelFlowService.ConfirmNextLevel();
        }
        private void Menu()
        {
            _levelFlowService.RequestReturnToMenu();
        }
    }
}
