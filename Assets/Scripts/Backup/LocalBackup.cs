namespace RehvidGames.Backup
{
    using System;
    using System.IO;
    using Exceptions;
    using UnityEngine;

    public class LocalBackup: IBackup
    {
        private const string BackupExtension = ".backup";
        
        public void CreateBackup(string fileName)
        {
            try
            {
                File.Copy(fileName, GetBackupPath(fileName), true);
            }
            catch (Exception e)
            {
                throw new BackupException("Failed to create backup.", e);
            }
        }

        public void DeleteBackup(string fileName)
        {
            if (!IsBackupExists(fileName, out string backupFilePath)) return;

            TryToDeleteBackup(backupFilePath);
        }

        private void TryToDeleteBackup(string backupFilePath)
        {
            try
            {
                File.Delete(backupFilePath);
            }
            catch (Exception e )
            {
                throw new BackupException("Failed to delete backup.", e);
            }
        }
        
        public void RestoreBackup(string fullPath)
        {
            if (!IsBackupExists(fullPath, out string backupFilePath)) return;
         
            TryToRestoreBackup(fullPath, backupFilePath);
        }
        
        private void TryToRestoreBackup(string fullPath, string backupFilePath)
        {
            try
            {
                File.Copy(fullPath, backupFilePath, true);
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            catch (Exception e)
            {
                throw new BackupException("Error occured when trying to roll backup file at :" + backupFilePath + "\n", e);
            }
        }

        private bool IsBackupExists(string fullPath, out string backupFilePath)
        {
            backupFilePath = GetBackupPath(fullPath);

            if (File.Exists(backupFilePath)) return true;
            
            Debug.LogWarning($"Rollback failed: no backup file found at path: {backupFilePath}");
            return false;
        }
        
        private string GetBackupPath(string fullPath)
        {
            return fullPath + BackupExtension;
        }
    }
}