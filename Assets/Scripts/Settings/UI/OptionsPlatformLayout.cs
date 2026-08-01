using OrderRushKitchen.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace OrderRushKitchen.Settings
{
    public class OptionsPlatformLayout : MonoBehaviour
    {
        [SerializeField] private PlatformUiVisibility.Mode mode;
        [SerializeField] private RectTransform panel;
        [SerializeField] private VerticalLayoutGroup layoutGroup;
        [SerializeField] private Vector2 mobilePanelSize = new(700f, 700f);
        [SerializeField] private RectOffset mobilePadding;
        [SerializeField] private float mobileSpacing = 20f;

        private void Awake()
        {
            if (!ShouldUseMobileLayout())
                return;

            panel.anchorMin = new Vector2(0.5f, 0.5f);
            panel.anchorMax = new Vector2(0.5f, 0.5f);
            panel.pivot = new Vector2(0.5f, 0.5f);
            panel.anchoredPosition = Vector2.zero;
            panel.sizeDelta = mobilePanelSize;

            layoutGroup.padding = mobilePadding ?? new RectOffset(112, 112, 96, 64);
            layoutGroup.spacing = mobileSpacing;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
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
