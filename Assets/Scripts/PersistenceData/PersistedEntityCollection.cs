namespace RehvidGames.PersistenceData
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class PersistedEntityCollection
    {
        [SerializeField] private string id;
        [SerializeField] private List<PersistedEntity> persistedEntities;

        public string Id => id;
        
        public PersistedEntityCollection(string id, List<PersistedEntity> persistedEntities)
        {
            this.id = id;
            this.persistedEntities = persistedEntities;
        }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> saveData = new();
            foreach (PersistedEntity entity in persistedEntities)
            {
                saveData[entity.Type] = entity.SerializedContent;
            }
            
            return saveData;
        }

        public PersistedEntity FindByType(string type)
        {
            return persistedEntities.Find(entity => entity.Type == type);
        }
    }
}