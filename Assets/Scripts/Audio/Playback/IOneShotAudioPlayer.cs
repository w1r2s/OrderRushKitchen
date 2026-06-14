using UnityEngine;

namespace OrderRushKitchen.Audio
{
    public interface IOneShotAudioPlayer
    {
        void Play(AudioClip clip, Vector3 position, float volume);
        void PlayGlobal(AudioClip clip, float volume);
    }
}
