namespace RehvidGames.PersistenceData
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class PersistedEntityCollection
    {
        public string Id { get; }
        public List<PersistedEntity> Entities { get; } = new();
        
        public PersistedEntityCollection(string id)
        {
            Id = id;
        }

        public void AddEntity(PersistedEntity entity)
        {
            Entities.Add(entity);
        }
        
        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> saveData = new();
            foreach (PersistedEntity entity in Entities)
            {
                saveData[entity.EntityType] = entity.Data;
            }
            
            return saveData;
        }

        public PersistedEntity FindByType(string type)
        {
            return Entities.Find(entity => entity.EntityType == type);
        }
    }
}