namespace RehvidGames.Factory
{
    using System;
    using Enums;
    using Serializer;
    using StorageWriter;

    public static class StorageWriterFactory
    {
        public static IStorageWriter Create(SerializationFormat format, ISerializer serializer)
        {
            return format switch
            {
                SerializationFormat.Json => new JsonStorageWriter(serializer),
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }
    }
}