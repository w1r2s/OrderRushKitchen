using UnityEngine;

namespace OrderRushKitchen.Counters
{

    public class StoveBurnWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private void Start()
        {
            stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            Hide();
        }

        private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            float burnShowProgressAmound = .5f;
            bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmound;
            if (show)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        private void Show()
        {
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}
