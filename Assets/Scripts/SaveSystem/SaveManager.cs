namespace RehvidGames.SaveSystem
{
    using Config;
    using DataStorage;
    using Entity;
    using Enums;
    using Factory;
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
        
        private static SaveManager instance;
        
        private IDataStorage dataStorage;

        public ISerializer Serializer { get; private set; }
      
        public static SaveManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SaveManager>();
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject("SaveManager");
                        instance = singletonObject.AddComponent<SaveManager>();
                    }
                }
                return instance;
            }
        }
        
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
        }

        private void Start()
        {
            Serializer = SerializerFactory.Create(serializationFormat);
            dataStorage = DataStorageFactory.Create(
                storageLocation, 
                StorageSettingsProvierFactory.Create(storageLocation, CreateFileSettings())
            );
        }
        
        public void SaveAll()
        {
            dataStorage.SaveAll();
        }
        
        private BaseSettings CreateFileSettings()
        {
            return new BaseSettings(
                configuration.GetConfigEntries(),
                FindSaveableEntities(),
                UseEncryption,
                Serializer,
                StorageWriterFactory.Create(serializationFormat, Serializer), 
                BackupFactory.Create(storageLocation)
            );
        }
        
        private SaveableEntity[] FindSaveableEntities()
        {
            return FindObjectsByType<SaveableEntity>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }
        
        public void LoadAll()
        {
            dataStorage.LoadAll();
        }
        
        public void LoadCategory(SaveFileCategory category)
        {
            dataStorage.LoadCategory(category);
        }
        
        public void LoadRecord(SaveRecord record)
        {
            dataStorage.LoadRecord(record);
        }
        
        public void SaveRecord(SaveRecord record)
        {
            dataStorage.SaveRecord(record);
        }
        
        public void SaveCategory(SaveFileCategory category)
        {
            dataStorage.SaveCategory(category);
        }

        public void DeleteAll()
        {
            dataStorage.DeleteAll();
        }
        
        public void DeleteCategory(SaveFileCategory category)
        {
            dataStorage.DeleteCategory(category);
        }
    }
}
