namespace RehvidGames.Backup
{
    public interface IBackup
    {
        public void CreateBackup(string fileName);
        public void RestoreBackup(string fullPath);
        public void DeleteBackup(string fullPath);
    }
}