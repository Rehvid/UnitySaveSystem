namespace RehvidGames.Backup
{
    public interface IBackup
    {
        public void CreateBackup(string fileName);
        public bool RestoreBackup(string fullPath);
    }
}