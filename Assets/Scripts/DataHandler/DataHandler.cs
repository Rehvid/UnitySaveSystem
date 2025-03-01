namespace RehvidGames.DataHandler
{
    using System.Collections.Generic;
    using System.Linq;
    using Config;
    using PersistenceData;
    using Providers;
    using SaveSystem;
    using Settings;
    using UnityEngine;

    public abstract class DataHandler: IDataHandler
    {
        protected readonly BaseSettings settings;
        protected readonly Dictionary<SaveCategory, string> saveCategories;
        
        private readonly SaveableEntity[] saveableEntities;
        
        protected DataHandler(IHandlerProvider provider)
        {
            settings = provider.GetSettings();
            
            saveCategories = settings.SaveCategories;
            saveableEntities = settings.SaveableEntities;
            
        }
        
        public void SaveAllData()
        {
            var persistedEntities = saveableEntities
                .GroupBy(entity => entity.Category)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Select(BuildPersistedEntityCollection).ToList()
                );
            
            foreach (var (category, entities) in persistedEntities)
            {
                if (!TryGetFileNameFromCategory(category, out var fileName)) continue;
                
                SaveData(fileName, entities);
            }
        }
        
        private PersistedEntityCollection BuildPersistedEntityCollection(SaveableEntity entity)
        {
            PersistedEntityCollection entityCollection = new PersistedEntityCollection(entity.Id);
                
            foreach (var saveableObject in entity.CaptureSaveableObjects())
            {
                entityCollection.AddEntity(new PersistedEntity(saveableObject.Value, saveableObject.Key));
            }
            
            return entityCollection;
        }
        
        private bool TryGetFileNameFromCategory(SaveCategory category, out string fileName)
        {
            if (settings.SaveCategories.TryGetValue(category, out fileName)) return true;
            
            Debug.LogError($"Not found file for category: {category}");
            return false;
        }
        
        protected abstract void SaveData(string path, object data);

        public void UpdateRecord(SaveRecord saveRecord)
        {
            if (!TryGetFileNameFromCategory(saveRecord.Category, out string fileName)) return;
           
            List<PersistedEntityCollection> collections = ReadPersistedCollections(fileName);
           
            if (collections == null)
            {
                Debug.LogError($"Cannot retrieve data for entity type '{saveRecord.EntityType}' from file: {fileName}");
                return;
            }
           
            PersistedEntity entity = collections
                .FirstOrDefault(collection => collection.Id == saveRecord.Id)
                ?.FindByType(saveRecord.EntityType);
           
            if (entity == null)
            {
                Debug.Log("Cannot find persisted entity");
                return;
            }
           
            entity.Data = saveRecord.Value;
            
            SaveData(fileName, entity);
        }

        protected abstract List<PersistedEntityCollection> ReadPersistedCollections(string fileName);

        public void SaveCategoryData(SaveCategory category)
        {
            if (!TryGetFileNameFromCategory(category, out var fileName)) return;
            
            List<PersistedEntityCollection> persistedEntityCollections = saveableEntities
                .Where(entity => entity.Category == category)
                .Select(BuildPersistedEntityCollection)
                .ToList();
            
            SaveData(fileName, persistedEntityCollections);
        }

        public void LoadAll()
        {
            foreach (var configEntry in settings.SaveCategories)
            {
                LoadData(configEntry.Value, saveableEntities);
            }
        }

        protected abstract void LoadData(string path, SaveableEntity[] entities);

        public void LoadCategory(SaveCategory category)
        {
            if (TryGetFileNameFromCategory(category, out var saveFileName)) return;
            
            LoadData(saveFileName, saveableEntities.Where(entity => entity.Category == category).ToArray());
        }

        public void LoadSingleValueInCategory(SaveRecord saveRecord)
        {
            if (!TryGetPersistedCollection(saveRecord, out var collection, out var saveFileName)) return;
            
            PersistedEntity entity = collection.FindByType(saveRecord.EntityType);
            if (entity == null)
            {
                Debug.LogError($"Cannot retrieve data from file: {saveFileName}");
                return;
            }

            saveableEntities
                .FirstOrDefault(saveableEntity => saveableEntity?.Id == collection.Id)
                ?.RestoreSingleSaveableObject(entity);
        }
        
        private bool TryGetPersistedCollection(
            SaveRecord record, 
            out PersistedEntityCollection collection,
            out string saveFileName
        )
        {
            collection = null;
            saveFileName = null;
            
            if (!TryGetFileNameFromCategory(record.Category, out saveFileName)) return false;
            
            var collections = ReadPersistedCollections(saveFileName);
            if (collections == null)
            {
                Debug.LogError($"Cannot retrieve data from file: {saveFileName}");
                return false;
            }
            
            collection = collections.Find(collection => collection?.Id == record.Id);
            if (collection == null)
            {
                Debug.LogError($"Cannot retrieve data for entity type '{record.EntityType}' from file: {saveFileName}");
                return false;
            }
            
            return true;
        }
    }
}