using Assets.Scripts.Managers.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI
{
    public class PauseButtonUI : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;

        private IGameService _gameService;

        [Inject]
        private void Construct(IGameService gameService)
        {
            _gameService = gameService;
        }

        private void Start()
        {
            pauseButton.onClick.AddListener(OnPauseClicked);
        }

        private void OnDestroy()
        {
            if (pauseButton != null)
                pauseButton.onClick.RemoveListener(OnPauseClicked);
        }

        private void OnPauseClicked()
        {
            _gameService.TogglePauseGame();
        }
    }

}
