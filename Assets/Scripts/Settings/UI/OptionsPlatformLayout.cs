using OrderRushKitchen.Presentation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrderRushKitchen.Settings
{
    public class OptionsPlatformLayout : MonoBehaviour
    {
        [SerializeField] private PlatformUiVisibility.Mode mode;
        [SerializeField] private RectTransform panel;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private RectTransform commonOptionsSection;
        [SerializeField] private RectTransform closeButton;
        [SerializeField] private Vector2 mobilePanelSize = new(900f, 900f);
        [SerializeField] private Vector2 mobilePanelPosition = Vector2.zero;
        [SerializeField] private RectOffset mobilePadding;
        [SerializeField] private float mobileSpacing = 24f;
        [SerializeField] private float mobileTitleMinFontSize = 64f;
        [SerializeField] private float mobileTitleMaxFontSize = 68f;
        [SerializeField] private Vector2 mobileTitleSize = new(650f, 96f);
        [SerializeField] private Vector4 mobileTitleMargin = new(8f, 0f, 8f, 0f);
        [SerializeField] private Vector2 mobileButtonSize = new(420f, 76f);
        [SerializeField] private float mobileButtonSpacing = 24f;
        [SerializeField] private float mobileButtonMinFontSize = 30f;
        [SerializeField] private float mobileButtonMaxFontSize = 46f;
        [SerializeField] private float mobileButtonHorizontalPadding = 28f;

        private void Awake()
        {
            if (!ShouldUseMobileLayout())
                return;

            panel.anchorMin = new Vector2(0.5f, 0.5f);
            panel.anchorMax = new Vector2(0.5f, 0.5f);
            panel.pivot = new Vector2(0.5f, 0.5f);
            panel.anchoredPosition = mobilePanelPosition;
            panel.sizeDelta = mobilePanelSize;

            layoutGroup.padding = mobilePadding ?? new RectOffset(160, 160, 180, 100);
            layoutGroup.spacing = mobileSpacing;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childControlWidth = false;
            layoutGroup.childForceExpandWidth = false;

            titleText.enableAutoSizing = true;
            titleText.fontSizeMin = mobileTitleMinFontSize;
            titleText.fontSizeMax = mobileTitleMaxFontSize;
            titleText.fontSize = mobileTitleMaxFontSize;
            titleText.margin = mobileTitleMargin;
            LayoutElement titleLayout = titleText.GetComponent<LayoutElement>();
            if (titleLayout == null)
                titleLayout = titleText.gameObject.AddComponent<LayoutElement>();
            titleLayout.preferredWidth = mobileTitleSize.x;
            titleLayout.preferredHeight = mobileTitleSize.y;
            titleText.rectTransform.sizeDelta = mobileTitleSize;

            commonOptionsSection.sizeDelta = new Vector2(mobileButtonSize.x, commonOptionsSection.sizeDelta.y);

            VerticalLayoutGroup commonOptionsLayout = commonOptionsSection.GetComponent<VerticalLayoutGroup>();
            commonOptionsLayout.childControlWidth = false;
            commonOptionsLayout.childForceExpandWidth = false;
            commonOptionsLayout.childAlignment = TextAnchor.UpperCenter;
            commonOptionsLayout.spacing = mobileButtonSpacing;

            foreach (RectTransform button in commonOptionsSection)
            {
                button.sizeDelta = mobileButtonSize;
                ApplyMobileButtonTypography(button);
            }

            closeButton.sizeDelta = mobileButtonSize;
            ApplyMobileButtonTypography(closeButton);
            if (closeButton.TryGetComponent(out LayoutElement closeButtonLayout))
            {
                closeButtonLayout.preferredWidth = mobileButtonSize.x;
                closeButtonLayout.preferredHeight = mobileButtonSize.y;
            }
        }

        private void ApplyMobileButtonTypography(RectTransform button)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);
            if (buttonText == null)
                return;

            buttonText.enableAutoSizing = true;
            buttonText.fontSizeMin = mobileButtonMinFontSize;
            buttonText.fontSizeMax = mobileButtonMaxFontSize;
            buttonText.fontSize = mobileButtonMaxFontSize;
            buttonText.fontStyle = FontStyles.Bold;
            buttonText.textWrappingMode = TextWrappingModes.NoWrap;

            RectTransform textRect = buttonText.rectTransform;
            textRect.offsetMin = new Vector2(mobileButtonHorizontalPadding, textRect.offsetMin.y);
            textRect.offsetMax = new Vector2(-mobileButtonHorizontalPadding, textRect.offsetMax.y);
        }

        private bool ShouldUseMobileLayout()
        {
            return mode switch
            {
                PlatformUiVisibility.Mode.Desktop => false,
                PlatformUiVisibility.Mode.Mobile => true,
                _ => Application.isMobilePlatform
            };
        }
    }
}
