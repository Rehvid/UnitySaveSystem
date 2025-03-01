namespace RehvidGames.Providers
{
    using Settings;

    public class FileDataHandlerProvider: IHandlerProvider
    {
        private readonly BaseSettings settings;
        
        public FileDataHandlerProvider(BaseSettings settings) 
        {
            this.settings = settings;
        }

        public BaseSettings GetSettings() => settings;
    }
}