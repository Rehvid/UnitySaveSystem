namespace RehvidGames.Factory
{
    using System;
    using Enums;
    using Serializer;

    public static class SerializerFactory
    {
        public static ISerializer Create(SerializerType type)
        {
            return type switch
            {
                SerializerType.Json => new JsonSerializer(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}