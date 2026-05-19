using Assets.Scripts.Managers.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.UI
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
