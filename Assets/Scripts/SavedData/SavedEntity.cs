namespace RehvidGames.SavedData
{
    using System;

    [Serializable]
    public class SavedEntity
    {
        public string EntityType { get; set; }
        public object Data { get; set; }
        
        public SavedEntity(string type, object data)
        {
            EntityType = type;
            Data = data;
        }
        
    }
}