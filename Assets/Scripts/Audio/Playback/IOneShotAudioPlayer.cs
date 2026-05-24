using UnityEngine;

namespace Assets.Scripts.Audio
{
    public interface IOneShotAudioPlayer
    {
        void Play(AudioClip clip, Vector3 position, float volume);
    }
}
