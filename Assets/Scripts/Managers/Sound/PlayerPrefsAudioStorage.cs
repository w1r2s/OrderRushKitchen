using UnityEngine;

namespace Assets.Scripts.Managers.Sound
{
    public class PlayerPrefsAudioStorage : IAudioStorage
    {
        private const string KEY = "PlayerSoundEffectsVolume";

        public float LoadVolume()
        {
            return PlayerPrefs.GetFloat(KEY, 1f);
        }

        public void SaveVolume(float volume)
        {
            PlayerPrefs.SetFloat(KEY, volume);
            PlayerPrefs.Save();
        }
    }
}
