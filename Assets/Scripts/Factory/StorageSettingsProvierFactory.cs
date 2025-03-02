namespace RehvidGames.Factory
{
    using System;
    using Enums;
    using Providers;
    using Settings;

    public static class StorageSettingsProvierFactory
    {
        public static IStorageSettingsProvider Create(StorageLocation location, BaseSettings settings)
        {
            return location switch
            {
                StorageLocation.Local => new LocalStorageSettingsProvider(settings),
                _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
            };
        }
    }
}