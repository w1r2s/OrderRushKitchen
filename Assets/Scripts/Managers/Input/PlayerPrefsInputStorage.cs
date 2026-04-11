using UnityEngine;

namespace Assets.Scripts.Managers.Input
{


    public class PlayerPrefsInputStorage : IInputStorage
    {
        private const string KEY = "PlayerKeyBindings";

        public void SaveBindings(string json)
        {
            PlayerPrefs.SetString(KEY, json);
            PlayerPrefs.Save();
        }

        public string LoadBindings()
        {
            return PlayerPrefs.HasKey(KEY) ? PlayerPrefs.GetString(KEY) : null;
        }
    }
}
