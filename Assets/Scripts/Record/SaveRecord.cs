namespace RehvidGames.Record
{
    using Enums;

    public class SaveRecord
    {
        public readonly SaveFileCategory FileCategory;
        public readonly string Id;
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