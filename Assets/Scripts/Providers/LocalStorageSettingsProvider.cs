namespace RehvidGames.Providers
{
    using Settings;

    public class LocalStorageSettingsProvider: IStorageSettingsProvider
    {
        private readonly BaseSettings settings;
        
        public LocalStorageSettingsProvider(BaseSettings settings) 
        {
            this.settings = settings;
        }

        public BaseSettings GetSettings() => settings;
    }
}