using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace OrderRushKitchen.Game
{
    public class HowToPlayUI : MonoBehaviour
    {
        private const string UiTable = "UI";
        private const string StartButtonKey = "ui.how_to_play.start";
        private const string BackButtonKey = "ui.common.back";

        [Header("Panels")]
        [SerializeField] private GameObject dimmer;
        [SerializeField] private GameObject panel;

        [Header("Platform Controls")]
        [SerializeField] private GameObject mobileControlsRoot;
        [SerializeField] private GameObject desktopControlsRoot;

        [Header("Desktop Bindings")]
        [SerializeField] private TextMeshProUGUI moveUpBindingText;
        [SerializeField] private TextMeshProUGUI moveDownBindingText;
        [SerializeField] private TextMeshProUGUI moveLeftBindingText;
        [SerializeField] private TextMeshProUGUI moveRightBindingText;
        [SerializeField] private TextMeshProUGUI interactBindingText;
        [SerializeField] private TextMeshProUGUI alternateInteractBindingText;

        [Header("Action")]
        [SerializeField] private Button actionButton;
        [SerializeField] private LocalizeStringEvent actionButtonLocalizer;

        public event EventHandler ActionRequested;

        public bool IsOpen => panel.activeSelf;

        private void Awake()
        {
            actionButton.onClick.AddListener(OnActionClicked);
            SetVisible(false);
        }

        private void OnDestroy()
        {
            if (actionButton != null)
                actionButton.onClick.RemoveListener(OnActionClicked);
        }

        public void Show(HowToPlayOpenMode mode)
        {
            bool useMobileControls = Application.isMobilePlatform;
            mobileControlsRoot.SetActive(useMobileControls);
            desktopControlsRoot.SetActive(!useMobileControls);

            string actionKey = mode == HowToPlayOpenMode.Automatic
                ? StartButtonKey
                : BackButtonKey;

            actionButtonLocalizer.StringReference.SetReference(UiTable, actionKey);
            actionButtonLocalizer.RefreshString();

            SetVisible(true);
            actionButton.Select();
        }

        public void Hide()
        {
            SetVisible(false);
        }

        public void SetDesktopBindings(
            string moveUp,
            string moveDown,
            string moveLeft,
            string moveRight,
            string interact,
            string alternateInteract)
        {
            moveUpBindingText.text = moveUp;
            moveDownBindingText.text = moveDown;
            moveLeftBindingText.text = moveLeft;
            moveRightBindingText.text = moveRight;
            interactBindingText.text = interact;
            alternateInteractBindingText.text = alternateInteract;
        }

        private void SetVisible(bool visible)
        {
            dimmer.SetActive(visible);
            panel.SetActive(visible);
        }

        private void OnActionClicked()
        {
            ActionRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
