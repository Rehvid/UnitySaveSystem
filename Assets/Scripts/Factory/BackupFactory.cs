namespace RehvidGames.Factory
{
    using System;
    using Backup;
    using Enums;

    public static class BackupFactory
    {
        public static IBackup Create(StorageLocation location)
        {
            return location switch
            {
                StorageLocation.Local => new LocalBackup(),
                _ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
            };
        }
    }
}