using UnityEngine;

namespace Assets.Scripts.Navigation
{
    public class LoadingScreenUI : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private GameObject panel;

        private float _progress;

        private void Awake()
        {
            Hide();
        }
        public void Hide()
        {
            panel.SetActive(false);
        }
        public void SetProgress(float progress)
        {
            _progress = Mathf.Clamp01(progress);
        }

        public void Show()
        {
            panel.SetActive(true);
        }
    }
}