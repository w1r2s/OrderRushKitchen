using UnityEngine;

namespace OrderRushKitchen.PlayerControl
{
    [RequireComponent(typeof(Player))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private Player _player;

        private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void LateUpdate()
        {
            _animator.SetBool(IsWalkingHash, _player.IsWalking());
        }
    }
}
