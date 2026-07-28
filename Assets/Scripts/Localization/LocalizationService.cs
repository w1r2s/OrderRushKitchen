using System;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace OrderRushKitchen.Localization
{
    public class LocalizationService : ILocalizationService, IInitializable, IDisposable
    {
        private const string DefaultLocaleCode = "en";

        private readonly ILocalizationStorage _storage;
        private AsyncOperationHandle<LocalizationSettings> _initializationOperation;
        private string _savedLocaleCode;
        private bool _isWaitingForInitialization;
        private bool _isSubscribedToLocaleChanges;

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
            _savedLocaleCode = _storage.LoadLocaleCode();
            _initializationOperation = LocalizationSettings.InitializationOperation;

            if (_initializationOperation.IsDone)
            {
                HandleLocalizationInitialized(_initializationOperation);
                return;
            }

            _isWaitingForInitialization = true;
            _initializationOperation.Completed += HandleLocalizationInitialized;
        }

        public void Dispose()
        {
            if (_isWaitingForInitialization && _initializationOperation.IsValid())
                _initializationOperation.Completed -= HandleLocalizationInitialized;

            if (_isSubscribedToLocaleChanges)
                LocalizationSettings.SelectedLocaleChanged -= HandleSelectedLocaleChanged;
        }

        private void HandleLocalizationInitialized(AsyncOperationHandle<LocalizationSettings> operation)
        {
            _isWaitingForInitialization = false;

            LocalizationSettings.SelectedLocaleChanged += HandleSelectedLocaleChanged;
            _isSubscribedToLocaleChanges = true;

            if (TrySetLocale(_savedLocaleCode))
                return;

            TrySetLocale(DefaultLocaleCode);
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
