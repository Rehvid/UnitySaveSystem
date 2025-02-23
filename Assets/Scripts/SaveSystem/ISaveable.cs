namespace RehvidGames.SaveSystem
{
    public interface ISaveable
    {
        public object Save();
        public void Load(object state);
    }
}