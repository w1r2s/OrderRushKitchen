using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;


    private float footstepTimer;
    [SerializeField] private float footstepTimerMax = 0.3f;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0)
        {
            footstepTimer = footstepTimerMax;
            if (player.IsWalking())
            {
                float volume = 1;
                SoundManager.Instance.PlayFootstepSound(player.transform.position, volume);
            }
        }
    }

}
