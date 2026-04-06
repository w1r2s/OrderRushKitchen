using Assets.Scripts;
using UnityEngine;
using Zenject;

public class SoundManager : MonoBehaviour
{

    private IDeliveryService _deliveryService;
    private Player _player;

    [SerializeField] private AudioClipRefsSo audioClipRefsSo;
    private float volume = 1f;
    private void Awake()
    {

       // Instance = this;
        volume = PlayerPrefs.GetFloat("PlayerSoundEffectsVolume", 1f);
    }

    [Inject]
    private void Construct(IDeliveryService deliveryService, Player player)
    {
        _deliveryService = deliveryService;
        _player = player;
    }
    private void Start()
    {
        _deliveryService.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        _deliveryService.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
       // CuttingCounter.onAnyCut += CuttingCounter_onAnyCut;
        _player.onPickedSomething += Player_onPickedSomething;
        BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSo.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlaced(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSo.objectDrop, baseCounter.transform.position);
    }

    private void Player_onPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSo.objectPickup, _player.transform.position);
    }

    private void CuttingCounter_onAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSo.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
      //  DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
       // PlaySound(audioClipRefsSo.deliveryFailed, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
      //  DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
      //  PlaySound(audioClipRefsSo.deliverySuccess, deliveryCounter.transform.position);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSo.footstep, position, volume);
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSo.warning, position);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1.1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat("PlayerSoundEffectsVolume", volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}