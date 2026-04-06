using UnityEngine;

namespace Assets.Scripts.Managers.Sound
{
    public interface IAudioService
    {
        void PlaySound(AudioClip clip, Vector3 position, float volume = 1f);
        void PlayFootstep(Vector3 position, float volume = 1f);
        void PlayCut(Vector3 position, float volume = 1f);
        void PlayWarning(Vector3 position);
        void PlayDrop(Vector3 position, float volume = 1f);
        void PlayTrash(Vector3 position, float volume = 1f);
        void PlayRecipeSuccess(Vector3 position, float volume = 1f);
        void PlayRecipeFail(Vector3 position, float volume = 1f);
        void PlayPickUp(Vector3 position, float volume = 1f);
        void ChangeVolume();
        float GetVolume();
    }
}
