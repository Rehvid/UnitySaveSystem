namespace RehvidGames.DataHandler
{
    using Config;

    public interface IDataHandler
    {
        public void SaveAll();
        public void OverwriteValueInCategory(SaveCategory category, string id, string type, object value);
        public void SaveCategory(SaveCategory category);
        public void LoadAll();
        public void LoadCategory(SaveCategory category);
        public void LoadSingleValueInCategory(SaveCategory category, string id, string type);
    }
}