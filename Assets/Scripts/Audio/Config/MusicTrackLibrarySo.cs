using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    [CreateAssetMenu(menuName = "Kitchen/Music Track Library")]
    public class MusicTrackLibrarySo : ScriptableObject
    {
        [SerializeField] private List<MusicTrackEntry> tracks;

        public bool TryGetClip(MusicTrackId trackId, out AudioClip clip)
        {
            clip = null;

            if (tracks == null)
                return false;

            for (int i = 0; i < tracks.Count; i++)
            {
                var entry = tracks[i];
                if (entry == null || entry.TrackId != trackId)
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
