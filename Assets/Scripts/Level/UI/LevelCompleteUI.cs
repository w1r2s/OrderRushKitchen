using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
using Zenject;

namespace OrderRushKitchen.Level
{
    public class LevelCompleteUI : MonoBehaviour
    {
        private ILevelCompletionFlowService _levelFlowService;

        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject dimmer;
        [SerializeField] private LocalizeStringEvent titleLocalizer;
        [SerializeField] private TextMeshProUGUI ordersDeliveredValueText;
        [SerializeField] private TextMeshProUGUI ordersFailedValueText;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button nextButton;

        private readonly IntVariable _levelVariable = new();

        [Inject]
        private void Construct(ILevelCompletionFlowService levelFlowService)
        {
            _levelFlowService = levelFlowService;
        }

        private void Start()
        {
            if (_levelFlowService == null)
                return;

            if (panel == null || dimmer == null || nextButton == null || titleLocalizer == null || ordersDeliveredValueText == null || ordersFailedValueText == null)
                return;

            _levelFlowService.OnCompletionShown += LevelFlowService_OnCompletionShown;
            _levelFlowService.OnCompletionHidden += LevelFlowService_OnCompletionHidden;

            nextButton.onClick.AddListener(Next);

            if (menuButton != null)
                menuButton.onClick.AddListener(Menu);

            Hide();
        }

        private void Awake()
        {
            if (titleLocalizer != null)
                titleLocalizer.StringReference.Add("level", _levelVariable);
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
            _levelVariable.Value = e.LevelIndex;
            titleLocalizer.RefreshString();

            ordersDeliveredValueText.text = e.CompletedOrders.ToString();
            ordersFailedValueText.text = e.FailedOrders.ToString();
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
