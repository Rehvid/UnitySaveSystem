namespace RehvidGames.DataStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;
    using Providers;
    using Record;
    using SaveSystem;
    using Settings;
    using SavedData;
    using UnityEngine;

    public abstract class BaseDataStorage: IDataStorage
    {
        protected readonly BaseSettings settings;
        protected readonly Dictionary<SaveFileCategory, string> fileCategoryToFileName;
        
        private readonly SaveableEntity[] saveableEntities;
        
        protected BaseDataStorage(IStorageSettingsProvider provider)
        {
            settings = provider.GetSettings();
            
            fileCategoryToFileName = settings.FileCategoryToFileName;
            saveableEntities = settings.SaveableEntities;
        }
        
        protected abstract void CreateBackup(string filePath);
        protected abstract List<SavedEntityCollection> ReadPersistedCollections(string filePath);
        protected abstract void LoadData(string filePath, SaveableEntity[] entities);
        protected abstract void SaveData(string filePath, object data);
        protected abstract string GetFilePath(string fileName);
        protected abstract void DeleteFile(string filePath);
        
        public void SaveAll()
        {
            var entitiesToPersist = GroupEntitiesByCategory(); 
            
            foreach (var (category, entities) in entitiesToPersist)
            {
                if (!TryGetFileNameFromCategory(category, out var fileName)) continue;
                
                SaveData(GetFilePath(fileName), entities);
            }
        }

        private Dictionary<SaveFileCategory, List<SavedEntityCollection>> GroupEntitiesByCategory()
        {
            return saveableEntities
                .GroupBy(entity => entity.FileCategory)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Select(BuildPersistedEntityCollection).ToList()
                );
        }
        
        private SavedEntityCollection BuildPersistedEntityCollection(SaveableEntity entity)
        {
            SavedEntityCollection entityCollection = new SavedEntityCollection(entity.Id);
                
            foreach (var (entityType, saveableData) in entity.CaptureSaveableObjects())
            {
                entityCollection.AddEntity(new SavedEntity(entityType, saveableData));
            }
            
            return entityCollection;
        }
        
        private bool TryGetFileNameFromCategory(SaveFileCategory fileCategory, out string fileName)
        {
            if (settings.FileCategoryToFileName.TryGetValue(fileCategory, out fileName)) return true;
            
            Debug.LogError($"Not found file for category: {fileCategory}");
            return false;
        }
        
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
            
            SaveData(GetFilePath(fileName), collections);
        }
        
        public void SaveCategory(SaveFileCategory fileCategory)
        { 
            if (!TryGetFileNameFromCategory(fileCategory, out var fileName)) return;
            
            List<SavedEntityCollection> persistedEntityCollections = saveableEntities
                .Where(entity => entity.FileCategory == fileCategory)
                .Select(BuildPersistedEntityCollection)
                .ToList();
            
            SaveData(GetFilePath(fileName), persistedEntityCollections);
        }

        public void LoadAll()
        {
            foreach (var configEntry in settings.FileCategoryToFileName)
            {
                LoadData(GetFilePath(configEntry.Value), saveableEntities);
            }
        }
        
        public void LoadCategory(SaveFileCategory fileCategory)
        {
            if (!TryGetFileNameFromCategory(fileCategory, out var fileName)) return;
            SaveableEntity[] entities = saveableEntities.Where(entity => entity.FileCategory == fileCategory).ToArray();
            
            LoadData(GetFilePath(fileName), entities);
        }

        public void LoadRecord(SaveRecord saveRecord)
        {
            if (!TryGetPersistedCollection(saveRecord, out var collection, out var fileName)) return;
            
            SavedEntity entity = collection.FindByType(saveRecord.EntityType);
            if (entity == null)
            {
                Debug.LogError($"Cannot retrieve data from file: {fileName}");
                return;
            }
            
            SaveableEntity saveableEntity = FindPersistedEntityInCollection(saveableEntities, collection);
            saveableEntity?.RestoreSingleSaveableObject(entity);
        }

        protected SaveableEntity FindPersistedEntityInCollection(SaveableEntity[] entities, SavedEntityCollection collection)
        {
            var saveableEntity = entities.FirstOrDefault(entity => entity?.Id == collection.Id);

            if (saveableEntity != null) return saveableEntity;
            
            Debug.LogWarning($"No matching saveable entity found for collection with Id: {collection.Id}");
            return null;
        }

        private bool TryGetPersistedCollection(
            SaveRecord record, 
            out SavedEntityCollection collection,
            out string fileName
        )
        {
            collection = null;
            fileName = null;
            
            if (!TryGetFileNameFromCategory(record.FileCategory, out fileName)) return false;
            
            var collections = ReadPersistedCollections(GetFilePath(fileName));
            
            if (collections == null)
            {
                Debug.LogError($"Cannot retrieve data from file: {fileName}");
                return false;
            }
            
            collection = collections.Find(collection => collection?.Id == record.Id);
            if (collection == null)
            {
                Debug.LogError($"Cannot retrieve data for entity type '{record.EntityType}' from file: {fileName}");
                return false;
            }
            
            return true;
        }
        
        public void DeleteAll()
        {
            foreach (var (category, filename) in settings.FileCategoryToFileName)
            {
                DeleteFile(GetFilePath(filename));
            }
        }

        public void DeleteCategory(SaveFileCategory fileCategory)
        {
            if (!TryGetFileNameFromCategory(fileCategory, out var fileName)) return;
            
            DeleteFile(GetFilePath(fileName));
        }

        protected List<SavedEntityCollection> TryRestoreFromBackup(string filePath)
        {
            try
            {
                settings.Backup.RestoreBackup(filePath);
                
                var backUpPersistedCollections = ReadPersistedCollections(filePath);

                if (backUpPersistedCollections != null) return backUpPersistedCollections;
                
                Debug.LogError($"Cannot retrieve date from backup file: {filePath}");
                return null;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return null;
            }
        }
    }
}