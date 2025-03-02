namespace RehvidGames.DataStorage
{
    using Enums;
    using Record;

    public interface IDataStorage
    {
        public void SaveAll();
        public void SaveRecord(SaveRecord saveRecord);
        public void SaveCategory(SaveFileCategory fileCategory);
        public void LoadAll();
        public void LoadCategory(SaveFileCategory fileCategory);
        public void LoadRecord(SaveRecord saveRecord);
    }
}