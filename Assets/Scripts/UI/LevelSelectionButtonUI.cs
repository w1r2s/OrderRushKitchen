using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class LevelSelectionButtonUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private GameObject lockedRoot;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField, Range(0f, 1f)] private float lockedAlpha = 0.45f;

        private int _levelNumber;
        private bool _isUnlocked;
        private Action<int> _clicked;

        private void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();

            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClick);
        }

        public void Bind(int levelNumber, bool isUnlocked, Action<int> clicked)
        {
            _levelNumber = levelNumber;
            _isUnlocked = isUnlocked;
            _clicked = clicked;

            if (levelText != null)
            {
                levelText.text = levelNumber.ToString();
                levelText.gameObject.SetActive(isUnlocked);
            }

            if (lockedRoot != null)
                lockedRoot.SetActive(!isUnlocked);

            if (button != null)
                button.interactable = isUnlocked;

            if (canvasGroup != null)
                canvasGroup.alpha = isUnlocked ? 1f : lockedAlpha;
        }

        private void OnClick()
        {
            if (!_isUnlocked)
                return;

            _clicked?.Invoke(_levelNumber);
        }
    }
}