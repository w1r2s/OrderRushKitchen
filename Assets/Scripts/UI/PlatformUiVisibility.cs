using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PlatformUiVisibility : MonoBehaviour
    {
        public enum Mode
        {
            Auto,
            Desktop,
            Mobile
        }

        [SerializeField] private Mode mode;
        [SerializeField] private GameObject[] desktopOnlyRoots;
        [SerializeField] private GameObject[] mobileOnlyRoots;


        private void Start()
        {
            switch (mode)
            {
                case Mode.Auto:
                    if (Application.isMobilePlatform)
                    {
                        ApplyMobile();
                    }
                    else
                    {
                        ApplyDesktop();
                    }
                    break;
                case Mode.Desktop:
                    ApplyDesktop();
                    break;
                case Mode.Mobile:
                    ApplyMobile();
                    break;
            }
        }
        private void ApplyMobile()
        {
            foreach (var root in desktopOnlyRoots)
            {
                if(root == null)
                    continue;

                root.SetActive(false);
            }

            foreach (var root in mobileOnlyRoots)
            {
                if (root == null)
                    continue;

                root.SetActive(true);
            }

        }
        private void ApplyDesktop()
        {
            foreach (var root in mobileOnlyRoots)
            {
                if (root == null)
                    continue;

                root.SetActive(false);
            }

            foreach (var root in desktopOnlyRoots)
            {
                if (root == null)
                    continue;

                root.SetActive(true);
            }
        }
    }
}
