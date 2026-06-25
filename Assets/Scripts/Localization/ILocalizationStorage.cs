namespace OrderRushKitchen.Localization
{
    public interface ILocalizationStorage
    {
        string LoadLocaleCode();
        void SaveLocaleCode(string localeCode);
    }
}
