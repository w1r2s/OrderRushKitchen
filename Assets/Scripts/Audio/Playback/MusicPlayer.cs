using UnityEngine;

namespace OrderRushKitchen.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour, IMusicPlayer
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
            _audioSource.spatialBlend = 0f;
            _audioSource.clip = null;
        }

        public void Play(AudioClip clip)
        {
            if (clip == null)
                return;

            if (_audioSource.clip == clip && _audioSource.isPlaying)
                return;

            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Stop()
        {
            if (!_audioSource.isPlaying && _audioSource.clip == null)
                return;

            _audioSource.Stop();
            _audioSource.clip = null;
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = Mathf.Clamp01(volume);
        }
    }
}