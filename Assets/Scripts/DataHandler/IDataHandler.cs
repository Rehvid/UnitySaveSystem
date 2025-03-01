namespace RehvidGames.DataHandler
{
    using Config;

    public interface IDataHandler
    {
        public void SaveAllData();
        public void UpdateRecord(SaveRecord saveRecord);
        public void SaveCategoryData(SaveCategory category);
        public void LoadAll();
        public void LoadCategory(SaveCategory category);
        public void LoadSingleValueInCategory(SaveRecord saveRecord);
    }
}