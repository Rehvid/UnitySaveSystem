namespace RehvidGames.DataHandler
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Config;
    using NewSystem;
    using PersistenceData;
    using UnityEngine;

    public class FileDataHandler
    {
        private Dictionary<SaveCategory, string> saveCategories;
        private SaveableEntity[] saveableEntities;
        
        public FileDataHandler(Dictionary<SaveCategory, string> saveCategories, SaveableEntity[] saveableEntities)
        {
            this.saveCategories = saveCategories;
            this.saveableEntities = saveableEntities;
        }
        
        public void SaveAll()
        {
            Dictionary<string, List<PersistedEntity>> persistedEntityDictionary = new();
            Dictionary<SaveCategory, Dictionary<string, List<PersistedEntity>>> persistedEntitiesBySaveCategory = new();

            foreach (SaveableEntity entity in saveableEntities)
            {
                Collect(entity, persistedEntityDictionary);

                if (!persistedEntitiesBySaveCategory.ContainsKey(entity.Category))
                {
                    persistedEntitiesBySaveCategory[entity.Category] = new Dictionary<string, List<PersistedEntity>>();
                }
                
                persistedEntitiesBySaveCategory[entity.Category][entity.Id] = persistedEntityDictionary[entity.Id];
            }

            WriteToFilesByCategory(persistedEntitiesBySaveCategory);
        }
        
        private void WriteToFilesByCategory(Dictionary<SaveCategory, Dictionary<string, List<PersistedEntity>>> persistedEntitiesBySaveCategory)
        {
            foreach (var entity in persistedEntitiesBySaveCategory)
            {
                if (!saveCategories.TryGetValue(entity.Key, out string fileName)) continue;
                
                WriteToFile(fileName, SerializeContent(new PersistedEntityRegistry(entity.Value)));
            }
        }
        
        private void Collect(SaveableEntity entity, Dictionary<string, List<PersistedEntity>> persistedEntityDictionary)
        {
            if (!persistedEntityDictionary.ContainsKey(entity.Id))
            {
                persistedEntityDictionary[entity.Id] = new List<PersistedEntity>();
            }
            
            foreach (var saveableObject in entity.CaptureSaveableObjects())
            {
                persistedEntityDictionary[entity.Id].Add(
                    CreatePersistedEntity(saveableObject.Value, saveableObject.Key)
                );
            }
        }

        private string SerializeContent(object value)
        {
            return JsonUtility.ToJson(value, true);
        }
        
        private void WriteToFile(string fileName, string serializedData)
        {
            File.WriteAllText(GetPathToFile(fileName), serializedData);
        }

        private string ReadFromFile(string fileName)
        {
            return File.ReadAllText(GetPathToFile(fileName));
        }

        public void OverwriteValueInCategory(SaveCategory category, string id, string type, object value)
        {
            if (!saveCategories.TryGetValue(category, out var saveFileName))
            {
                Debug.LogError($"Brak pliku dla kategorii: {category}");
                return;
            }
             
            string content = ReadFromFile(saveFileName);
            if (string.IsNullOrEmpty(content))
            {
                Debug.LogError($"Plik {saveFileName} jest pusty lub nie istnieje.");
                return;
            }
            
            PersistedEntityRegistry persistedEntityRegistry = JsonUtility.FromJson<PersistedEntityRegistry>(content);
            if (persistedEntityRegistry == null)
            {
                Debug.LogError($"Nie udało się sparsować danych z pliku {saveFileName}");
                return;
            }

            Dictionary<string, PersistedEntityCollection> saveData = persistedEntityRegistry.GetSaveData();
            if (!saveData.TryGetValue(id, out PersistedEntityCollection entityCollection))
            {
                Debug.LogError($"Nie znaleziono ID: {id} w kategorii: {category}");
                return;
            }
            
           PersistedEntity persistedEntity = entityCollection.FindByType(type);
           if (persistedEntity == null)
           {
               Debug.LogError($"Nie znaleziono wpisu dla typu: {type} w ID: {id}");
               return;
           }
           
            persistedEntity.SetSerializeContent(SerializeContent(value));
            persistedEntityRegistry.SetSaveData(saveData);
            
            WriteToFile(saveFileName, SerializeContent(persistedEntityRegistry));
        }

        public void SaveCategory(SaveCategory category)
        {
            if (!saveCategories.TryGetValue(category, out var saveFileName))
            {
                Debug.LogError($"Brak pliku dla kategorii: {category}");
                return;
            }
            
            SaveableEntity[] entities = saveableEntities.Where(entity => entity.Category == category).ToArray();
            Dictionary<string, List<PersistedEntity>> persistedEntityDictionary = new();
            
            foreach (SaveableEntity entity in entities)
            {
                Collect(entity, persistedEntityDictionary);
            }
            
            WriteToFile(saveFileName, SerializeContent(new PersistedEntityRegistry(persistedEntityDictionary)));
        }

        
        public void LoadAll()
        {
            foreach (var configEntry in saveCategories)
            {
                LoadContentFromFile(configEntry.Value, saveableEntities);
            }
        }

        public void LoadCategory(SaveCategory category)
        {
            if (!saveCategories.TryGetValue(category, out var saveFileName))
            {
                Debug.LogError($"Brak pliku dla kategorii: {category}");
                return;
            }
            SaveableEntity[] entities = saveableEntities.Where(entity => entity.Category == category).ToArray();
            
            LoadContentFromFile(saveFileName, entities);
        }
        
        public void LoadSingleValueInCategory(SaveCategory category, string id, string type)
        {
            if (!saveCategories.TryGetValue(category, out var saveFileName))
            {
                Debug.LogError($"Brak pliku dla kategorii: {category}");
                return;
            }
            
            string content = ReadFromFile(saveFileName);
            PersistedEntityRegistry saveProvider = JsonUtility.FromJson<PersistedEntityRegistry>(content);
            Dictionary<string, PersistedEntityCollection> saveData = saveProvider.GetSaveData();

            if (!saveData.TryGetValue(id, out PersistedEntityCollection entityCollection))
            {
                Debug.LogError(" ");
                return;
            }
            
            PersistedEntity persistedEntity = entityCollection.FindByType(type);
            if (persistedEntity == null)
            {
                Debug.LogError(" ");
                return;
            }

            SaveableEntity ent = saveableEntities.First(entity => entity.Id == id);
            if (ent == null)
            {
                Debug.LogError(" ");
                return;
            }
            
            ent.RestoreSingleSaveableObject(persistedEntity);
            Debug.Log("LoadSingleValueInCategory" + ent.Id);
        }

        private void LoadContentFromFile(string fileName, SaveableEntity[] entities)
        {
            string content = ReadFromFile(fileName);
                
            PersistedEntityRegistry saveProvider = JsonUtility.FromJson<PersistedEntityRegistry>(content);
            Dictionary<string, PersistedEntityCollection> saveData = saveProvider.GetSaveData();
                
            foreach (SaveableEntity entity in entities)
            {
                if (saveData.TryGetValue(entity.Id, out PersistedEntityCollection value))
                {
                    entity.RestoreSaveableObjects(value.ToDictionary());
                }
            }
        }
        
        private PersistedEntity CreatePersistedEntity(object value, string key)
        {
            return new PersistedEntity(JsonUtility.ToJson(value), key);
        }
        
        private string GetPathToFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}