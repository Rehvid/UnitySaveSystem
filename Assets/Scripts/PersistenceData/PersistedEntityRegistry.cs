namespace RehvidGames.PersistenceData
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class PersistedEntityRegistry
    {
        [SerializeField] private List<PersistedEntityCollection> entityCollections = new();  
        
        public Dictionary<string, PersistedEntityCollection> GetSaveData()
        {
            Dictionary<string, PersistedEntityCollection> saveData = new();
            
            foreach (PersistedEntityCollection entity in entityCollections)
            {
                saveData[entity.Id] = entity;
            }
            
            return saveData;
        }

        public PersistedEntityRegistry(Dictionary<string, List<PersistedEntity>> dictionary)
        {
            foreach (var data in dictionary)
            {
                entityCollections.Add(new PersistedEntityCollection(data.Key, data.Value));
            }
        }
        
        public PersistedEntity FindPersistedEntity(string id, string type)
        {
            PersistedEntityCollection collection = entityCollections.Find(entity => entity.Id == id);
            return collection?.FindByType(type);
        }

        public void SetSaveData( Dictionary<string, PersistedEntityCollection> saveData)
        {
            entityCollections.Clear();
            foreach (var data in saveData)
            {
                entityCollections.Add(data.Value);
            }
        }
    }
}
