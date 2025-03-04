namespace RehvidGames.Record
{
    using Enums;

    [System.Serializable]
    public class SaveRecord
    {
        public SaveFileCategory FileCategory;
        public string Id;
        public readonly string EntityType;
        public readonly object Value;

        public SaveRecord(SaveFileCategory fileCategory, string id, string type, object value = null)
        {
            FileCategory = fileCategory;
            Id = id;
            EntityType = type;
            Value = value;
        }
    }
}