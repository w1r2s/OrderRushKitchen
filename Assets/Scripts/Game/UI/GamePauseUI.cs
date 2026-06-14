using System;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Game
{

    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button optionsButton;

        private IGamePauseService _pauseService;

        public event EventHandler ResumeRequested;
        public event EventHandler MainMenuRequested;
        public event EventHandler OptionsRequested;

        private bool _isSuppressed;

        [Inject]
        private void Construct(IGamePauseService pauseService)
        {
            _pauseService = pauseService;
        }

        private void Start()
        {
            _pauseService.OnPauseChanged += PauseService_OnPauseChanged;

            resumeButton.onClick.AddListener(Resume);

            mainMenuButton.onClick.AddListener(ReturnToMenu);

            optionsButton.onClick.AddListener(OpenOptions);

            RefreshVisibility();
        }

        private void OnDestroy()
        {
            if (_pauseService != null)
            {
                _pauseService.OnPauseChanged -= PauseService_OnPauseChanged;
            }
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.RemoveListener(ReturnToMenu);
            }
            if (optionsButton != null)
            {
                optionsButton.onClick.RemoveListener(OpenOptions);
            }
            if (resumeButton != null)
            {
                resumeButton.onClick.RemoveListener(Resume);
            }
        }
        private void PauseService_OnPauseChanged(object sender, EventArgs e)
        {
            RefreshVisibility();
        }

        public void SetSuppressed(bool suppressed)
        {
            _isSuppressed = suppressed;
            RefreshVisibility();
        }

        private void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        private void RefreshVisibility()
        {
            SetVisible(_pauseService.HasPause(GamePauseReason.UserPause) && !_isSuppressed);
        }

        private void Resume()
        {
            ResumeRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OpenOptions()
        {
            OptionsRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ReturnToMenu()
        {
            MainMenuRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
