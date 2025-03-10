﻿namespace RehvidGames.StorageWriter
{
    using System.IO;
    using Encryption;
    using Serializer;
    using UnityEngine;

    public class JsonStorageWriter: IStorageWriter
    {
        private readonly ISerializer serializer;
        
        public JsonStorageWriter(ISerializer serializer)
        {
            this.serializer = serializer;
        }
        
        public bool Save(string path, object data, bool useEncryption)
        {
            var jsonData = serializer.Serialize(data);
            
            if (jsonData is not string serializedData)
            {
                Debug.LogError("Can't save data to json file");
                return false;
            }
           
            if (useEncryption)
            {
                serializedData = SaveEncryption.Encrypt(serializedData);
            } 
                
            File.WriteAllText(path, serializedData);
            
            return true;
        }

        public T Load<T>(string path, bool useEncryption)
        { 
            if (!File.Exists(path))
            {
                Debug.LogError("Can't load data from json file");
                return default;
            }

            var jsonData = File.ReadAllText(path);

            if (string.IsNullOrEmpty(jsonData))
            {
                Debug.LogError($"File {path} is empty or unreadable.");
                return default;
            }

            if (useEncryption)
            {
                jsonData = SaveEncryption.Decrypt(jsonData);
            }
            
            return serializer.Deserialize<T>(jsonData);
        }
    }
}