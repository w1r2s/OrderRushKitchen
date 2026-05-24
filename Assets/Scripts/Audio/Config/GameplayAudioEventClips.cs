using System;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    [Serializable]
    public class GameplayAudioEventClips
    {
        public GameplayAudioEvent EventType;
        public AudioClip[] Clips;
    }
}
