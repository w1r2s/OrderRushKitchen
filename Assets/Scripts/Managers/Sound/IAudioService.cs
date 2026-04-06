using UnityEngine;

namespace Assets.Scripts.Managers.Sound
{
    public interface IAudioService
    {
        void PlaySound(AudioClip clip, Vector3 position, float volume = 1f);
        void PlayFootstep(Vector3 position, float volume);
        void PlayCut(Vector3 position, float volume);
        void PlayWarning(Vector3 position);

        void ChangeVolume();
        float GetVolume();
    }
}
