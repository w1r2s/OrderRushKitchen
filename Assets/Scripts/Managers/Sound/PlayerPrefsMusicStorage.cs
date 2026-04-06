using UnityEngine;

namespace Assets.Scripts.Managers.Sound
{
    public class PlayerPrefsMusicStorage : IMusicStorage
    {
        private const string KEY = "PlayerMusicVolume";

        public float LoadVolume()
        {
            return PlayerPrefs.GetFloat(KEY, 0.3f);
        }

        public void SaveVolume(float volume)
        {
            PlayerPrefs.SetFloat(KEY, volume);
            PlayerPrefs.Save();
        }
    }
}
