namespace RehvidGames.Config
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SaveConfig", menuName = "Save System/Save Configuration")]
    public class SaveConfig: ScriptableObject
    {
        [SerializeField] private List<SaveConfigEntry> configEntries = new();

        public Dictionary<SaveCategory, string> GetConfigEntries()
        {
            Dictionary<SaveCategory, string> result = new();
            foreach (SaveConfigEntry configEntry in this.configEntries)
            {
                if (!result.ContainsKey(configEntry.Category))
                {
                    result.Add(configEntry.Category, configEntry.FileName);
                }
            }
            
            return result;
        }
        
        public SaveConfigEntry GetConfigEntry(SaveCategory category) => configEntries.Find(config => config.Category == category);
    }
}