namespace RehvidGames.DataStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Newtonsoft.Json;
    using Providers;
    using Record;
    using SaveSystem;
    using Settings;
    using SavedData;
    using UnityEngine;

    public abstract class BaseDataStorage: IDataStorage
    {
        protected readonly BaseSettings settings;
        protected readonly Dictionary<SaveFileCategory, string> saveCategories;
        
        private readonly SaveableEntity[] saveableEntities;
        
        protected BaseDataStorage(IStorageSettingsProvider provider)
        {
            settings = provider.GetSettings();
            
            saveCategories = settings.SaveCategories;
            saveableEntities = settings.SaveableEntities;
        }
        
        public void SaveAll()
        {
            var persistedEntities = saveableEntities
                .GroupBy(entity => entity.FileCategory)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Select(BuildPersistedEntityCollection).ToList()
                );
            
            foreach (var (category, entities) in persistedEntities)
            {
                if (!TryGetFileNameFromCategory(category, out var fileName)) continue;
                
                SaveData(GetFullPath(fileName), entities);
            }
        }
        
        private SavedEntityCollection BuildPersistedEntityCollection(SaveableEntity entity)
        {
            SavedEntityCollection entityCollection = new SavedEntityCollection(entity.Id);
                
            foreach (var saveableObject in entity.CaptureSaveableObjects())
            {
                entityCollection.AddEntity(new SavedEntity(saveableObject.Value, saveableObject.Key));
            }
            
            return entityCollection;
        }
        
        private bool TryGetFileNameFromCategory(SaveFileCategory fileCategory, out string fileName)
        {
            if (settings.SaveCategories.TryGetValue(fileCategory, out fileName)) return true;
            
            Debug.LogError($"Not found file for category: {fileCategory}");
            return false;
        }
        
        protected abstract void SaveData(string path, object data);

        public void SaveRecord(SaveRecord saveRecord)
        {
            if (!TryGetFileNameFromCategory(saveRecord.FileCategory, out string fileName)) return;
           
            List<SavedEntityCollection> collections = ReadPersistedCollections(fileName);
            
            if (collections == null)
            {
                Debug.LogError($"Cannot retrieve data for entity type '{saveRecord.EntityType}' from file: {fileName}");
                return;
            }
           
            SavedEntity entity = collections
                .FirstOrDefault(collection => collection.Id == saveRecord.Id)
                ?.FindByType(saveRecord.EntityType);
           
            if (entity == null)
            {
                Debug.Log("Cannot find persisted entity");
                return;
            }
           
            entity.Data = saveRecord.Value;
            
            SaveData(GetFullPath(fileName), collections);
        }

        protected abstract List<SavedEntityCollection> ReadPersistedCollections(string fileName);

        public void SaveCategory(SaveFileCategory fileCategory)
        { 
            if (!TryGetFileNameFromCategory(fileCategory, out var fileName)) return;
            
            List<SavedEntityCollection> persistedEntityCollections = saveableEntities
                .Where(entity => entity.FileCategory == fileCategory)
                .Select(BuildPersistedEntityCollection)
                .ToList();
            
            SaveData(GetFullPath(fileName), persistedEntityCollections);
        }

        public void LoadAll()
        {
            foreach (var configEntry in settings.SaveCategories)
            {
                LoadData(GetFullPath(configEntry.Value), saveableEntities);
            }
        }

        protected abstract void LoadData(string path, SaveableEntity[] entities);

        public void LoadCategory(SaveFileCategory fileCategory)
        {
            if (!TryGetFileNameFromCategory(fileCategory, out var saveFileName)) return;
            
            LoadData(GetFullPath(saveFileName), saveableEntities.Where(entity => entity.FileCategory == fileCategory).ToArray());
        }

        public void LoadRecord(SaveRecord saveRecord)
        {
            if (!TryGetPersistedCollection(saveRecord, out var collection, out var saveFileName)) return;
            
            SavedEntity entity = collection.FindByType(saveRecord.EntityType);
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
            out SavedEntityCollection collection,
            out string saveFileName
        )
        {
            collection = null;
            saveFileName = null;
            
            if (!TryGetFileNameFromCategory(record.FileCategory, out saveFileName)) return false;
            
            var collections = ReadPersistedCollections(GetFullPath(saveFileName));
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

        protected abstract string GetFullPath(string fileName);
    }
}