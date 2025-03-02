namespace RehvidGames.SavedData
{
    using System.Collections.Generic;

    [System.Serializable]
    public class SavedEntityCollection
    {
        public string Id { get; }
        public List<SavedEntity> Entities { get; } = new();
        
        public SavedEntityCollection(string id)
        {
            Id = id;
        }

        public void AddEntity(SavedEntity entity)
        {
            Entities.Add(entity);
        }
        
        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> saveData = new();
            foreach (SavedEntity entity in Entities)
            {
                saveData[entity.EntityType] = entity.Data;
            }
            
            return saveData;
        }

        public SavedEntity FindByType(string type)
        {
            return Entities.Find(entity => entity.EntityType == type);
        }
    }
}