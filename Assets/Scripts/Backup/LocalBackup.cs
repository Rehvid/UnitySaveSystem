namespace RehvidGames.Backup
{
    using System;
    using System.IO;
    using Exceptions;
    using UnityEngine;

    public class LocalBackup: IBackup
    {
        private const string BackupExtension = ".backup";
        
        public void CreateBackup(string filePath)
        {
            try
            {
                File.Copy(filePath, GetBackupPath(filePath), true);
            }
            catch (Exception e)
            {
                throw new BackupException("Failed to create backup.", e);
            }
        }

        public void DeleteBackup(string filePath)
        {
            if (!TryGetExistedBackup(filePath, out string backupFilePath)) return;

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
        
        public void RestoreBackup(string filePath)
        {
            if (!TryGetExistedBackup(filePath, out string backupFilePath)) return;
         
            TryToRestoreBackup(filePath, backupFilePath);
        }
        
        private void TryToRestoreBackup(string filePath, string backupFilePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning("Source file doesn't exist, create new one: " + filePath);
                    using (FileStream fs = File.Create(filePath)) {} 
                }
                
                File.Copy(backupFilePath, filePath, true);
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            catch (Exception e) 
            {
                throw new BackupException("Error occured when trying to roll backup file at : " + backupFilePath + "\n" + e.Message, e);
            }
        }

        private bool TryGetExistedBackup(string filePath, out string backupFilePath)
        {
            backupFilePath = GetBackupPath(filePath);

            if (File.Exists(backupFilePath)) return true;
            
            Debug.LogWarning($"Rollback failed: no backup file found at path: {backupFilePath}");
            return false;
        }
        
        private string GetBackupPath(string filePath)
        {
            return filePath + BackupExtension;
        }
    }
}