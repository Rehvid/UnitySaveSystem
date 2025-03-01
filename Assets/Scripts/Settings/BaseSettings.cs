namespace RehvidGames.Settings
{
    using System.Collections.Generic;
    using Backup;
    using Config;
    using FileSaver;
    using SaveSystem;
    using Serializer;

    public class BaseSettings
    {
        public Dictionary<SaveCategory, string> SaveCategories { get; private set; }
        public SaveableEntity[] SaveableEntities { get; private set; }
        public bool UseEncryption { get; private set; }
        public ISerializer Serializer { get; private set; }
        
        public IFileSaver FileSaver { get; private set; }
        
        public IBackup Backup { get; private set; }
        
        public BaseSettings(
            Dictionary<SaveCategory, string> saveCategories,
            SaveableEntity[] saveableEntities,
            bool useEncryption,
            ISerializer serializer,
            IFileSaver fileSaver,
            IBackup backup
        )
        {
            SaveCategories = saveCategories;
            SaveableEntities = saveableEntities;
            UseEncryption = useEncryption;
            Serializer = serializer;
            FileSaver = fileSaver;
            Backup = backup;
        }
    }
}