namespace RehvidGames.Settings
{
    using System.Collections.Generic;
    using Backup;
    using Enums;
    using SaveSystem;
    using Serializer;
    using StorageWriter;

    public class BaseSettings
    {
        public Dictionary<SaveFileCategory, string> SaveCategories { get; private set; }
        public SaveableEntity[] SaveableEntities { get; private set; }
        public bool UseEncryption { get; private set; }
        public ISerializer Serializer { get; private set; }
        
        public IStorageWriter StorageWriter { get; private set; }
        
        public IBackup Backup { get; private set; }
        
        public BaseSettings(
            Dictionary<SaveFileCategory, string> saveCategories,
            SaveableEntity[] saveableEntities,
            bool useEncryption,
            ISerializer serializer,
            IStorageWriter storageWriter,
            IBackup backup
        )
        {
            SaveCategories = saveCategories;
            SaveableEntities = saveableEntities;
            UseEncryption = useEncryption;
            Serializer = serializer;
            StorageWriter = storageWriter;
            Backup = backup;
        }
    }
}