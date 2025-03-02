namespace RehvidGames.Factory
{
    using System;
    using Enums;
    using Serializer;

    public static class SerializerFactory
    {
        public static ISerializer Create(SerializationFormat type)
        {
            return type switch
            {
                SerializationFormat.Json => new JsonSerializer(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}