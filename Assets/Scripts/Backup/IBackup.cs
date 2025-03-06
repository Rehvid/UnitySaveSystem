namespace RehvidGames.Backup
{
    public interface IBackup
    {
        public void CreateBackup(string filePath);
        public void RestoreBackup(string filePath);
        public void DeleteBackup(string filePath);
    }
}