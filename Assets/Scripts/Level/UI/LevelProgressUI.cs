using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Level
{
    public class LevelProgressUI : MonoBehaviour
    {
        private ILevelProgressionService _progressionService;
        private ICurrentLevelProvider _currentLevelProvider;

        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI ordersProgressText;

        [Inject]
        private void Construct(
            ILevelProgressionService progressionService,
            ICurrentLevelProvider currentLevelProvider)
        {
            _progressionService = progressionService;
            _currentLevelProvider = currentLevelProvider;
        }

        private void Start()
        {
            if (_progressionService == null || _currentLevelProvider == null)
                return;

            if (levelNumberText == null || ordersProgressText == null)
                return;

            _progressionService.OnProgressChanged += ProgressionService_OnProgressChanged;
            _currentLevelProvider.OnCurrentLevelChanged += CurrentLevelProvider_OnCurrentLevelChanged;

            Refresh();
        }

        private void OnDestroy()
        {
            if (_progressionService != null)
                _progressionService.OnProgressChanged -= ProgressionService_OnProgressChanged;

            if (_currentLevelProvider != null)
                _currentLevelProvider.OnCurrentLevelChanged -= CurrentLevelProvider_OnCurrentLevelChanged;
        }

        private void ProgressionService_OnProgressChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void CurrentLevelProvider_OnCurrentLevelChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            levelNumberText.text = _progressionService.CurrentLevelIndex.ToString();
            ordersProgressText.text = $"{_progressionService.CompletedOrdersInLevel} / {_progressionService.OrdersToCompleteForCurrentLevel}";
        }
    }
}
