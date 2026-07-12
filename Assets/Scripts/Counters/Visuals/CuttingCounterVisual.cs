using UnityEngine;

namespace OrderRushKitchen.Counters
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private CuttingCounter cuttingCounter;

        private static readonly int cutHash = Animator.StringToHash("Cut");
        private void Start()
        {
            cuttingCounter.OnCut += CuttingCounter_OnCut;
        }
        private void OnDestroy()
        {
            if (cuttingCounter != null)
                cuttingCounter.OnCut -= CuttingCounter_OnCut;
        }

        private void CuttingCounter_OnCut(object sender, System.EventArgs e)
        {
            animator.SetTrigger(cutHash);
        }
    }
}
