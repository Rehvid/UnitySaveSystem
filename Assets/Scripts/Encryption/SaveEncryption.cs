namespace RehvidGames.Encryption
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;

    public static class SaveEncryption
    {
        private static EncryptionConfig config;
        
        private static void LoadConfig()
        {
            if (config == null)
            {
                config = Resources.Load<EncryptionConfig>("EncryptionConfig");
            }
        }
        
        public static string Encrypt(string plainText)
        {
            LoadConfig();
            
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(config.Key);
            aes.IV = Encoding.UTF8.GetBytes(config.IV);
        
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter writer = new(cryptoStream);
        
            writer.Write(plainText);
            writer.Close();
        
            return Convert.ToBase64String(memoryStream.ToArray());
        }
        
        public static string Decrypt(string cipherText)
        {
            try
            {
                LoadConfig();
                
                byte[] buffer = Convert.FromBase64String(cipherText);
        
                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(config.Key);
                aes.IV = Encoding.UTF8.GetBytes(config.IV);
        
                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using StreamReader reader = new(cryptoStream);
            
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.LogError($"Błąd podczas deszyfrowania: {e.Message}");
                return null;
            }
        }
    }
}