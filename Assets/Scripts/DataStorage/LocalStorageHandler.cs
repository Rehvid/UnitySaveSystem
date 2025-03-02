namespace RehvidGames.DataStorage
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Providers;
    using SaveSystem;
    using SavedData;
    using UnityEngine;

    public class LocalStorageHandler: BaseDataStorage
    {
        public LocalStorageHandler(IStorageSettingsProvider provider) : base(provider) { }
        
        protected override void SaveData(string fileName, object data)
        {
            bool saveResult = settings.StorageWriter.Save(GetPathToFile(fileName), data, settings.UseEncryption);
            
            if (saveResult)
            {
                settings.Backup.CreateBackup(GetPathToFile(fileName));   
            }
        }

        protected override List<SavedEntityCollection> ReadPersistedCollections(string fileName)
        {
            return settings.StorageWriter.Load<List<SavedEntityCollection>>(fileName, settings.UseEncryption);
        }
        
        protected override void LoadData(string fileName, SaveableEntity[] entities)
        {
            List<SavedEntityCollection> persistedCollections = ReadPersistedCollections(fileName);
            
            if (persistedCollections == null)
            {
                Debug.LogError($"Cannot retrieve date from file: {fileName}");
                return;
            }
            
            foreach (var collection in persistedCollections)
            {
                entities
                    .FirstOrDefault(saveableEntity => saveableEntity?.Id == collection.Id)
                    ?.RestoreSaveableObjects(collection.ToDictionary());
            }
        }
        
        private string GetPathToFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}