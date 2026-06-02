using Assets.Scripts.Level;
using Assets.Scripts.Navigation;
using Assets.Scripts.Progress;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [SerializeField] private GameObject selectionPanel;
        [SerializeField] private Button closeButton;
        [SerializeField] private LevelSelectionButtonUI levelButtonTemplate;
        [SerializeField] private Transform levelButtonsPanel;

        private readonly List<LevelSelectionButtonUI> _buttons = new();

        private LevelDatabase _levelDatabase;
        private IUserProgressService _progressSerivce;
        private INavigationService _navigationService;
        private bool _isLoading;

        public event EventHandler Closed;

        [Inject]
        private void Construct(LevelDatabase levelDatabase, IUserProgressService progressService, INavigationService navigationService)
        {
            _levelDatabase = levelDatabase;
            _progressSerivce = progressService;
            _navigationService = navigationService;
        }

        private void Start()
        {
            levelButtonTemplate.gameObject.SetActive(false);
            closeButton.onClick.AddListener(Hide);
            selectionPanel.SetActive(false);
        }
        private void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Hide);
            }
        }

        public void Show()
        {
            Rebuild();
            selectionPanel.SetActive(true);
        }

        public void Hide()
        {
            if (!selectionPanel.activeSelf)
                return;

            selectionPanel.SetActive(false);
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void Rebuild()
        {
            ClearButtons();
            var levels = _levelDatabase.GetAll().OrderBy(level => level.levelNumber);
            var unlockedLevel = _progressSerivce.UnlockedLevelNumber;

            foreach (var level in levels)
            {
                var button = Instantiate(levelButtonTemplate, levelButtonsPanel, false);
                button.gameObject.SetActive(true);
                button.Bind(level.levelNumber, level.levelNumber <= unlockedLevel, OnLevelClicked);
                _buttons.Add(button);
            }

        }

        private void OnLevelClicked(int levelNumber)
        {
            LoadLevelAsync(levelNumber).Forget(HandleLoadException);
        }

        private async UniTask LoadLevelAsync(int levelNumber)
        {
            if (_isLoading)
                return;
            if (levelNumber > _progressSerivce.UnlockedLevelNumber)
                return;

            _isLoading = true;
            _progressSerivce.SetCurrentLevel(levelNumber);
            await _navigationService.LoadGameAsync(this.GetCancellationTokenOnDestroy());
        }

        private static void HandleLoadException(Exception ex)
        {
            if (ex is OperationCanceledException)
                return;

            Debug.LogException(ex);
        }

        private void ClearButtons()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (_buttons[i] != null)
                    Destroy(_buttons[i].gameObject);
            }

            _buttons.Clear();
        }
    }
}
