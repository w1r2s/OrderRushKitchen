using System;
using UnityEngine;

namespace OrderRushKitchen.UserProgress
{
    public class PlayerPrefsUserProgressStorage : IUserProgressStorage
    {
        private const string CurrentLevelKey = "CurrentLevel";
        private const string UnlockedLevelKey = "UnlockedLevel";
        private const string HowToPlaySeenKey = "HowToPlaySeen";
        public UserProgressData Load()
        {
            var currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
            var unlockedLevel = PlayerPrefs.GetInt(UnlockedLevelKey, 1);
            var hasSeenHowToPlay = PlayerPrefs.GetInt(HowToPlaySeenKey, 0) == 1;

            if (currentLevel <= 0)
            {
                currentLevel = 1;
            }
            if (unlockedLevel <= 0)
            {
                unlockedLevel = 1;
            }

            if (currentLevel > unlockedLevel)
            {
                currentLevel = unlockedLevel;
            }

            return new UserProgressData(currentLevel, unlockedLevel, hasSeenHowToPlay);
        }

        public void Save(UserProgressData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            PlayerPrefs.SetInt(CurrentLevelKey, data.CurrentLevelNumber);
            PlayerPrefs.SetInt(UnlockedLevelKey, data.MaxUnlockedLevelNumber);
            PlayerPrefs.SetInt(HowToPlaySeenKey, data.HasSeenHowToPlay ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
