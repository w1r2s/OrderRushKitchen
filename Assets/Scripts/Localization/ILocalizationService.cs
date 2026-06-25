using System;
using System.Collections.Generic;

namespace OrderRushKitchen.Localization
{
    public interface ILocalizationService
    {
        event EventHandler OnLocaleChanged;

        string CurrentLocaleCode { get; }
        IReadOnlyList<string> AvailableLocaleCodes { get; }

        bool TrySetLocale(string localeCode);
        bool TrySelectNextLocale();
    }
}
