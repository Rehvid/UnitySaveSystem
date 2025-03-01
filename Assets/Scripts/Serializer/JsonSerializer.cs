namespace RehvidGames.Serializer
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    public sealed class JsonSerializer: ISerializer
    {
        public object Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T Deserialize<T>(object obj)
        {
            if (obj is string jsonString)
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            
            throw new SerializationException("Invalid input: expected a JSON string.");
        }
    }
}