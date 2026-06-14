using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace OrderRushKitchen.Game
{
    public class PauseButtonUI : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;

        private IGamePauseService _pauseService;

        [Inject]
        private void Construct(IGamePauseService pauseService)
        {
            _pauseService = pauseService;
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
            _pauseService.ToggleUserPause();
        }
    }

}
