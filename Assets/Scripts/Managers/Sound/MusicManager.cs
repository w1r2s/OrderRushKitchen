using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //TODO: пофиксить первоначальную громкость
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
