using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    [CreateAssetMenu(menuName = "Kitchen/Gameplay Audio Library")]
    public class GameplayAudioLibrarySo : ScriptableObject
    {
        [SerializeField] private List<GameplayAudioEventClips> eventClips;
        [SerializeField] private List<GameplayAudioLoopClip> loopClips;

        public bool TryGetEventClips(GameplayAudioEvent audioEvent, out IReadOnlyList<AudioClip> clips)
        {
            clips = null;

            if (eventClips == null)
                return false;

            for (int i = 0; i < eventClips.Count; i++)
            {
                var entry = eventClips[i];
                if (entry == null || entry.EventType != audioEvent)
                    continue;

                if (entry.Clips == null || entry.Clips.Length == 0)
                    return false;

                clips = entry.Clips;
                return true;
            }

            return false;
        }

        public bool TryGetLoopClip(GameplayAudioLoop audioLoop, out AudioClip clip)
        {
            clip = null;

            if (loopClips == null)
                return false;

            for (int i = 0; i < loopClips.Count; i++)
            {
                var entry = loopClips[i];
                if (entry == null || entry.LoopType != audioLoop)
                    continue;

                if (entry.Clip == null)
                    return false;

                clip = entry.Clip;
                return true;
            }

            return false;
        }
    }
}