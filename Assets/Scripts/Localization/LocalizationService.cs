using System;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;

namespace OrderRushKitchen.Localization
{
    public class LocalizationService : ILocalizationService, IInitializable, IDisposable
    {
        private const string DefaultLocaleCode = "en";

        private readonly ILocalizationStorage _storage;

        public event EventHandler OnLocaleChanged;
        public string CurrentLocaleCode => LocalizationSettings.SelectedLocale?.Identifier.Code ?? DefaultLocaleCode;

        public IReadOnlyList<string> AvailableLocaleCodes => GetAvailableLocales();

        [Inject]
        public LocalizationService(ILocalizationStorage storage)
        {
            _storage = storage;
        }

        public void Initialize()
        {
            LocalizationSettings.SelectedLocaleChanged += HandleSelectedLocaleChanged;

            string savedLocaleCode = _storage.LoadLocaleCode();

            if (TrySetLocale(savedLocaleCode))
                return;

            TrySetLocale(DefaultLocaleCode);
        }

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= HandleSelectedLocaleChanged;
        }

        private void HandleSelectedLocaleChanged(Locale locale)
        {
            if (locale != null)
                _storage.SaveLocaleCode(locale.Identifier.Code);

            OnLocaleChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool TrySelectNextLocale()
        {
            IReadOnlyList<string> localeCodes = AvailableLocaleCodes;

            if (localeCodes.Count == 0)
                return false;

            int currentIndex = -1;

            for (int i = 0; i < localeCodes.Count; i++)
            {
                if (string.Equals(localeCodes[i], CurrentLocaleCode, StringComparison.OrdinalIgnoreCase))
                {
                    currentIndex = i;
                    break;
                }
            }

            int nextIndex = currentIndex < 0 ? 0 : (currentIndex + 1) % localeCodes.Count;
            return TrySetLocale(localeCodes[nextIndex]);
        }

        public bool TrySetLocale(string localeCode)
        {
            if (string.IsNullOrWhiteSpace(localeCode))
                return false;

            Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode.Trim());

            if (locale == null)
                return false;

            if (LocalizationSettings.SelectedLocale == locale)
                return true;

            LocalizationSettings.SelectedLocale = locale;
            return true;
        }

        private IReadOnlyList<string> GetAvailableLocales()
        {
            var locales = LocalizationSettings.AvailableLocales?.Locales;

            if (locales == null || locales.Count == 0)
                return Array.Empty<string>();

            var localeCodes = new List<string>(locales.Count);

            for (int i = 0; i < locales.Count; i++)
            {
                Locale locale = locales[i];

                if (locale != null)
                {
                    localeCodes.Add(locale.Identifier.Code);
                }
            }

            return localeCodes;
        }
    }
}
