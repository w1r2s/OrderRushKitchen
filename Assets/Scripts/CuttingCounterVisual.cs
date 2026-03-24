using UnityEngine;

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
        cuttingCounter.onCut += CuttingCounter_onCut;
    }

    private void CuttingCounter_onCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger("Cut");
    }
}
