namespace RehvidGames.StorageWriter
{
    public interface IStorageWriter
    {
        bool Save(string path, object data, bool useEncryption);
        T Load<T>(string path, bool useEncryption);
    }
}