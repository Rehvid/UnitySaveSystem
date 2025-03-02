namespace RehvidGames.Providers
{
    using Settings;

    public interface IStorageSettingsProvider
    {
        BaseSettings GetSettings();
    }
}