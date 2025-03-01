namespace RehvidGames.FileSaver
{
    public interface IFileSaver
    {
        bool Save(string path, object data, bool useEncryption);
        T Load<T>(string path, bool useEncryption);
    }
}