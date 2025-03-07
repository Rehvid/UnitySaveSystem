namespace RehvidGames.DataStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Enums;
    using Providers;
    using SaveSystem;
    using SavedData;
    using UnityEngine;

    public class LocalStorageHandler: BaseDataStorage
    {
        public LocalStorageHandler(IStorageSettingsProvider provider) : base(provider) { }
        
        protected override void SaveData(string filePath, object data)
        {
            bool saveResult = settings.StorageWriter.Save(filePath, data, settings.UseEncryption);
            
            if (saveResult)
            {
                CreateBackup(filePath);
            } else
            {
                Debug.LogError($"Failed to save data to file: {filePath}");
            }
        }

        protected override void CreateBackup(string filePath)
        {
            try
            {
                settings.Backup.CreateBackup(filePath);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        protected override void DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                settings.Backup.DeleteBackup(filePath);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }

        protected override List<SavedEntityCollection> ReadPersistedCollections(string filePath, bool isBackupRead)
        {
            List<SavedEntityCollection> collections = settings.StorageWriter.Load<List<SavedEntityCollection>>(filePath, settings.UseEncryption);
            
            if (collections != null) return collections;
            
            Debug.LogError($"Cannot retrieve date from file: {filePath}");

            return isBackupRead ? null : TryRestoreFromBackup(filePath);
        }
        
        protected override void LoadData(string filePath, SaveableEntity[] entities)
        {
            List<SavedEntityCollection> persistedCollections = ReadPersistedCollections(filePath, false);
            
            if (persistedCollections == null || persistedCollections.Count <= 0)
            {
                Debug.LogError($"Cannot retrieve date from file: {filePath}");
                return;
            }
            
            foreach (var collection in persistedCollections)
            {
                RestoreSaveableObjects(entities, collection);
            }
        }

        private void RestoreSaveableObjects(SaveableEntity[] entities, SavedEntityCollection collection)
        {
            SaveableEntity saveableEntity = FindPersistedEntityInCollection(entities, collection);
            
            if (saveableEntity != null)
            {
                saveableEntity.RestoreSaveableObjects(collection.ToDeserializedDictionary(settings.Serializer));
            }
        }
        
        protected override string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}