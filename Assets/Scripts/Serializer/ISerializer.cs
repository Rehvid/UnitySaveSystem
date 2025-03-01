namespace RehvidGames.Serializer
{
    public interface ISerializer
    {
        public object Serialize(object obj);
        
        public T Deserialize<T>(object obj);
    }
}