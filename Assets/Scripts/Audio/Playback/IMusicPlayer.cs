using UnityEngine;

namespace Assets.Scripts.Audio
{
    public interface IMusicPlayer
    {
        void Play(AudioClip clip);
        void Stop();
        void SetVolume(float volume);
    }
}
