using UnityEngine;

namespace OrderRushKitchen.Counters
{

    public class CuttingCounterVisual : MonoBehaviour
    {
        private Animator animator;

        [SerializeField] private CuttingCounter cuttingCounter;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            cuttingCounter.OnCut += CuttingCounter_OnCut;
        }

        private void CuttingCounter_OnCut(object sender, System.EventArgs e)
        {
            animator.SetTrigger("Cut");
        }
        private void OnDestroy()
        {
            if (cuttingCounter != null)
                cuttingCounter.OnCut -= CuttingCounter_OnCut;
        }
    }

}
