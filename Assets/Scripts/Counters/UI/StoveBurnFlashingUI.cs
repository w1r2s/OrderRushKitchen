using UnityEngine;

namespace OrderRushKitchen.Counters
{

    public class StoveBurnFlashingUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        private Animator animator;

        private static readonly int IsFlashingHash = Animator.StringToHash("IsFlashing");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
            animator.SetBool(IsFlashingHash, false);
        }

        private void OnDestroy()
        {
            stoveCounter.OnProgressChanged -= StoveCounter_OnProgressChanged;
        }

        private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
        {
            float burnShowProgressAmound = .5f;
            bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmound;
            animator.SetBool(IsFlashingHash, show);
        }
    }

}
