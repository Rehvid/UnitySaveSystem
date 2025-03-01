namespace RehvidGames.DataHandler
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Backup;
    using FileSaver;
    using PersistenceData;
    using Providers;
    using UnityEngine;
    using SaveSystem;

    public class FileDataHandler: DataHandler
    {
        public FileDataHandler(IHandlerProvider provider) : base(provider) { }
        
        protected override void SaveData(string fileName, object data)
        {
            bool saveResult = settings.FileSaver.Save(GetPathToFile(fileName), data, settings.UseEncryption);
            
            if (saveResult)
            {
                settings.Backup.CreateBackup(GetPathToFile(fileName));   
            }
        }

        protected override List<PersistedEntityCollection> ReadPersistedCollections(string fileName)
        {
            return settings.FileSaver.Load<List<PersistedEntityCollection>>(fileName, settings.UseEncryption);
        }
        
        protected override void LoadData(string fileName, SaveableEntity[] entities)
        {
            List<PersistedEntityCollection> persistedCollections = ReadPersistedCollections(fileName);
            
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