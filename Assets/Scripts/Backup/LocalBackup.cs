namespace RehvidGames.Backup
{
    using System;
    using System.IO;
    using Exceptions;
    using UnityEngine;

    public class LocalBackup: IBackup
    {
        public void CreateBackup(string fileName)
        {
            try
            {
                File.Copy(fileName, GetBackupPath(fileName), true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create backup. Exception: {e}");
                throw new BackupException("Failed to create backup.", e);
            }
        }

        public bool RestoreBackup(string fullPath)
        {
            string backupFilePath = GetBackupPath(fullPath);

            if (!File.Exists(backupFilePath))
            {
                Debug.LogWarning($"Rollback failed: no backup file found at path: {backupFilePath}");
                return false;
            }

            try
            {
                File.Copy(fullPath, backupFilePath, true);
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to roll backup file at :" + backupFilePath + "\n" + e);
                return false;
            }
            
        }

        private string GetBackupPath(string fullPath)
        {
            return fullPath + ".backup";
        }
    }
}