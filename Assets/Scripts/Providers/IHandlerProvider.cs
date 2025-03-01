namespace RehvidGames.Providers
{
    using Settings;

    public interface IHandlerProvider
    {
        BaseSettings GetSettings();
    }
}