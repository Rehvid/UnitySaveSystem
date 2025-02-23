namespace RehvidGames.PersistenceData
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PersistedEntity
    {
        [SerializeField] private string serializedContent;
        [SerializeField] private string type;
        
        public string SerializedContent => serializedContent;

        public string Type => type;
        
        public PersistedEntity(string serializedContent, string type)
        {
            this.serializedContent = serializedContent;
            this.type = type;
        }

        public void SetSerializeContent(string content)
        {
            serializedContent = content;
        }
    }
}