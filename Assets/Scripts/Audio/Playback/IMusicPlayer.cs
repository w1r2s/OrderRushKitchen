using UnityEngine;

namespace OrderRushKitchen.Audio
{
    public interface IMusicPlayer
    {
        void Play(AudioClip clip);
        void Stop();
        void SetVolume(float volume);
    }
}
