using UnityEngine;

namespace OrderRushKitchen.Counters
{

    public class ContainerCounterVisual : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private ContainerCounter container;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            container.OnPlayerGrabbedObject += Container_OnPlayerGrabbedObject;
        }

        private void Container_OnPlayerGrabbedObject(object sender, System.EventArgs e)
        {
            animator.SetTrigger("OpenClose");
        }

    }

}
