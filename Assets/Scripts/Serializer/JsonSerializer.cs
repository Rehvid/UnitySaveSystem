namespace RehvidGames.Serializer
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using UnityEngine;

    public sealed class JsonSerializer: ISerializer
    {
        public object Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T Deserialize<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }
    }
}