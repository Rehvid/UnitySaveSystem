namespace RehvidGames.SaveSystem
{
    using Backup;
    using Config;
    using DataStorage;
    using Entity;
    using Enums;
    using Factory;
    using Providers;
    using Record;
    using Serializer;
    using Settings;
    using UnityEngine;

    public class SaveManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SaveConfiguration configuration;
        [SerializeField] private StorageLocation storageLocation = StorageLocation.Local;
        [SerializeField] private SerializationFormat serializationFormat = SerializationFormat.Json;
        [field: SerializeField] public bool UseEncryption { get; private set; }
        
        
        private IDataStorage dataStorage;

        private void Awake()
        {
            dataStorage = CreateDataHandler();
        }

        [ContextMenu("SaveAllData")]
        public void Save()
        {
            dataStorage = CreateDataHandler();
            dataStorage.SaveAll();
        }

        private IDataStorage CreateDataHandler()
        {
            return DataStorageFactory.Create(
                storageLocation,
                StorageSettingsProvierFactory.Create(storageLocation, CreateFileSettings())
            );
        }

        private BaseSettings CreateFileSettings()
        {
            ISerializer serializer = SerializerFactory.Create(serializationFormat);
            
            return new BaseSettings(
                configuration.GetConfigEntries(),
                FindSaveableEntities(),
                UseEncryption,
                serializer,
                StorageWriterFactory.Create(serializationFormat, serializer), 
                BackupFactory.Create(storageLocation)
            );
        }
        
        private SaveableEntity[] FindSaveableEntities()
        {
            return FindObjectsByType<SaveableEntity>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

        [ContextMenu("LoadAllFiles")]
        public void LoadAllFiles()
        {
            dataStorage = CreateDataHandler();
            dataStorage.LoadAll();
        }

        [ContextMenu("LoadCategory => ENEMIES")]
        public void LoadCategory()
        {
            dataStorage = CreateDataHandler();
            dataStorage.LoadCategory(SaveFileCategory.Enemies);
        }

        [ContextMenu("LoadSingleCategory => Player Health ")]
        public void LoadSingleCategory()
        {
            SaveRecord record = new SaveRecord(
                SaveFileCategory.Player, 
                "bdcc17d6-ccaf-4996-a0a9-a02f35708659",
                "RehvidGames.Entity.Health"
            );
            dataStorage = CreateDataHandler();
            
            dataStorage.LoadRecord(record);
        }

        [ContextMenu("OverwriteValueInCategory => Player, ID, Health, HealthSystem")]
        public void SaveValuesInCategory()
        {
            var obj = new HealthSystemData
            {
                CurrentHealth = 6899.1f,
                MaxHealth = 1232.2f
            };

            SaveRecord record = new SaveRecord(
                SaveFileCategory.Player,
                "bdcc17d6-ccaf-4996-a0a9-a02f35708659",
                "RehvidGames.Entity.Health",
                obj
            );

            dataStorage = CreateDataHandler();
            dataStorage.SaveRecord(record);
        }

        [ContextMenu("SaveCategory => Player")]
        public void SaveCat()
        {
            dataStorage = CreateDataHandler();
            dataStorage.SaveCategory(SaveFileCategory.Player);
        }
    }
}
