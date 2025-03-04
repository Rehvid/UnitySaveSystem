namespace RehvidGames.Config
{
    using System.Collections.Generic;
    using Enums;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SaveConfig", menuName = "Save System/Save Configuration")]
    public class SaveConfiguration: ScriptableObject
    {
        [SerializeField] private List<SaveFileConfigEntry> configEntries = new();

        public Dictionary<SaveFileCategory, string> GetConfigEntries()
        {
            Dictionary<SaveFileCategory, string> result = new();
            foreach (SaveFileConfigEntry configEntry in this.configEntries)
            {
                if (!result.ContainsKey(configEntry.fileCategory))
                {
                    result.Add(configEntry.fileCategory, configEntry.FileName);
                }
            }
            
            
            
            return result;
        }
    }
}