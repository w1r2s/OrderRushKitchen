using UnityEngine;

namespace OrderRushKitchen.Localization
{
    public class PlayerPrefsLocalizationStorage : ILocalizationStorage
    {
        private const string LocaleCodeKey = "Localization.LocaleCode";
        public string LoadLocaleCode()
        {
            if (!PlayerPrefs.HasKey(LocaleCodeKey))
                return null;

            string localeCode = PlayerPrefs.GetString(LocaleCodeKey);

            return string.IsNullOrWhiteSpace(localeCode) ? null : localeCode;
        }

        public void SaveLocaleCode(string localeCode)
        {
            if (string.IsNullOrWhiteSpace(localeCode))
                return;

            PlayerPrefs.SetString(LocaleCodeKey, localeCode);
            PlayerPrefs.Save();
        }
    }
}
