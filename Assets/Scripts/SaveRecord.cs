namespace RehvidGames
{
    using Config;

    public class SaveRecord
    {
        public readonly SaveCategory Category;
        public readonly string Id;
        public readonly string EntityType;
        public readonly object Value;

        public SaveRecord(SaveCategory category, string id, string type, object value = null)
        {
            Category = category;
            Id = id;
            EntityType = type;
            Value = value;
        }
    }
}