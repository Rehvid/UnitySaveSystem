namespace RehvidGames.SaveSystem
{
    using Backup;
    using Config;
    using DataHandler;
    using Entity;
    using Enums;
    using Factory;
    using FileSaver;
    using Providers;
    using Settings;
    using UnityEngine;

    public class SaveManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private SaveConfig config;
        [SerializeField] private DataHandlerType handlerType = DataHandlerType.File;
        [SerializeField] private SerializerType serializerType = SerializerType.Json;
        [field: SerializeField] public bool UseEncryption { get; private set; }
        
        
        private IDataHandler dataHandler;

        private void Awake()
        {
            dataHandler = CreateDataHandler();
        }

        [ContextMenu("SaveAllData")]
        public void Save()
        {
            dataHandler = CreateDataHandler();
            dataHandler.SaveAllData();
        }

        private IDataHandler CreateDataHandler()
        {
            return DataHandlerFactory.Create(
                handlerType,
                new FileDataHandlerProvider(CreateFileSettings())
            );
        }

        private BaseSettings CreateFileSettings()
        {
            return new BaseSettings(
                config.GetConfigEntries(),
                FindSaveableEntities(),
                UseEncryption,
                SerializerFactory.Create(serializerType),
                new JsonFileSaver(SerializerFactory.Create(serializerType)),
                new FileBackup()
            );
        }
        
        private SaveableEntity[] FindSaveableEntities()
        {
            return FindObjectsByType<SaveableEntity>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

        [ContextMenu("LoadAllFiles")]
        public void LoadAllFiles()
        {
            dataHandler = CreateDataHandler();
            dataHandler.LoadAll();
        }

        [ContextMenu("LoadCategory => ENEMIES")]
        public void LoadCategory()
        {
            dataHandler = CreateDataHandler();
            dataHandler.LoadCategory(SaveCategory.Enemies);
        }

        [ContextMenu("LoadSingleCategory => Player Health ")]
        public void LoadSingleCategory()
        {
            SaveRecord record = new SaveRecord(
                SaveCategory.Player, 
                "bdcc17d6-ccaf-4996-a0a9-a02f35708659",
                "RehvidGames.Entity.Health"
            );
            dataHandler = CreateDataHandler();
            
            dataHandler.LoadSingleValueInCategory(record);
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
                SaveCategory.Player,
                "bdcc17d6-ccaf-4996-a0a9-a02f35708659",
                "RehvidGames.Entity.Health",
                obj
            );

            dataHandler = CreateDataHandler();
            dataHandler.UpdateRecord(record);
        }

        [ContextMenu("SaveCategory => Player")]
        public void SaveCat()
        {
            dataHandler = CreateDataHandler();
            dataHandler.SaveCategoryData(SaveCategory.Player);
        }
    }
}
