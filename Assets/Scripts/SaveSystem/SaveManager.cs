namespace RehvidGames.SaveSystem
{
    using System;
    using Config;
    using DataHandler;
    using Entity;
    using NewSystem;
    using UnityEngine;

    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveConfig config;
        [field: SerializeField] public bool UseEncryption { get; private set; }
        
        private IDataHandler dataHandler;

        private void Awake()
        {
            dataHandler = CreateDataHandler();
        }

        [ContextMenu("SaveAllFiles")]
        public void Save()
        {
            dataHandler = CreateDataHandler();
            dataHandler.SaveAll();
        }

        private IDataHandler CreateDataHandler()
        {
            return new FileDataHandler(config.GetConfigEntries(), FindSaveableEntities(), UseEncryption);
        }
        
        private SaveableEntity[] FindSaveableEntities()
        {
            return FindObjectsByType<SaveableEntity>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

        [ContextMenu("LoadAllFiles")]
        public void LoadAllFiles()
        {
            dataHandler = CreateDataHandler();
            dataHandler.LoadAll();
        }

        [ContextMenu("LoadCategory => ENEMIES")]
        public void LoadCategory()
        {
            dataHandler = CreateDataHandler();
            dataHandler.LoadCategory(SaveCategory.Enemies);
        }

        [ContextMenu("LoadCategory => OverwriteValueInCategory")]
        public void LoadSingleCategory()
        {
            dataHandler = CreateDataHandler();
            dataHandler.LoadSingleValueInCategory(
                 SaveCategory.Player,
                "bdcc17d6-ccaf-4996-a0a9-a02f35708659",
                "RehvidGames.Entity.Health"
            );
        }

        [ContextMenu("OverwriteValueInCategory => Player, ID, Health, HealthSystem")]
        public void SaveValuesInCategory()
        {
            var obj = new HealthSystemData
            {
                CurrentHealth = 669.1f,
                MaxHealth = 888.2f
            };

            dataHandler = CreateDataHandler();
            dataHandler.OverwriteValueInCategory(
                SaveCategory.Player,
                "bdcc17d6-ccaf-4996-a0a9-a02f35708659",
                "RehvidGames.Entity.Health",
                obj
            );
        }

        [ContextMenu("SaveCategory")]
        public void SaveCat()
        {
            dataHandler = CreateDataHandler();
            dataHandler.SaveCategory(SaveCategory.Enemies);
        }
    }
}
