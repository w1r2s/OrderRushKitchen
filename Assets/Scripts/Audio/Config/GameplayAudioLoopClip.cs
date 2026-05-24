using System;
using UnityEngine;

namespace Assets.Scripts.Audio
{ 
    [Serializable]
    public class GameplayAudioLoopClip
    {
        public GameplayAudioLoop LoopType;
        public AudioClip Clip;
    }
}
