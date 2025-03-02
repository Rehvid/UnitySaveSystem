namespace RehvidGames.Factory
{
    using System;
    using DataStorage;
    using Enums;
    using Providers;

    public static class DataStorageFactory
    {
        public static IDataStorage Create(StorageLocation handlerLocation, IStorageSettingsProvider storageSettingsProvider)
        {
            return handlerLocation switch
            {
                StorageLocation.Local => new LocalStorageHandler(storageSettingsProvider),
                _ => throw new ArgumentOutOfRangeException(nameof(handlerLocation), handlerLocation, null)
            };
        }
    }
}