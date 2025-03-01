namespace RehvidGames.PersistenceData
{
    using System;

    [Serializable]
    public class PersistedEntity
    {
        public object Data { get; set; }
        public string EntityType { get; set; }
        
        public PersistedEntity(object data, string type)
        {
            Data = data;
            EntityType = type;
        }
    }
}