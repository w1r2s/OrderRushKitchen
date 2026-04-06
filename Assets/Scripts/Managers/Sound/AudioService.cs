using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Assets.Scripts.Managers.Sound
{
    public class AudioService : IAudioService
    {
        private readonly AudioClipRefsSo _clips;
        private readonly IAudioStorage _storage;

        private float volume;
        public AudioService(AudioClipRefsSo clips, IAudioStorage storage)
        {
            _clips = clips;
            _storage = storage;

            volume = _storage.LoadVolume();
        }
        public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
        }
        private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
        {
            PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
        }
        public void PlayFootstep(Vector3 position, float volume)
        {
            PlaySound(_clips.footstep, position, volume);
        }
        public void PlayWarning(Vector3 position)
        {
            PlaySound(_clips.warning, position);
        }
        public void PlayCut(Vector3 position, float volume = 1f)
        {
            PlaySound(_clips.chop, position, volume);
        }
        public void ChangeVolume()
        {
            volume += 0.1f;

            if (volume > 1.1f)
                volume = 0f;

            _storage.SaveVolume(volume);
        }
        public float GetVolume()
        {
            return volume;
        }
    }
}
