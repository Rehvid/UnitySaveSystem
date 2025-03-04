namespace RehvidGames.SavedData
{
    using System;

    [Serializable]
    public class SavedEntity
    {
        public object Data { get; set; }
        public string EntityType { get; set; }
        
        public SavedEntity(object data, string type)
        {
            Data = data;
            EntityType = type;
        }
        
    }
}