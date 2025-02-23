namespace RehvidGames.SaveSystem
{
    using Config;
    using DataHandler;
    using Entity;
    using NewSystem;
    using UnityEngine;

    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveConfig config;
        
        [ContextMenu("SaveAllFiles")]
        public void Save()
        {
            GetFileDataHandler().SaveAll();
        }

        private FileDataHandler GetFileDataHandler()
        {
            return new FileDataHandler(config.GetConfigEntries(), FindSaveableEntities());
        }
        
        private SaveableEntity[] FindSaveableEntities()
        {
            return FindObjectsByType<SaveableEntity>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        }

        [ContextMenu("LoadAllFiles")]
        public void LoadAllFiles()
        {
            GetFileDataHandler().LoadAll();
        }

        [ContextMenu("LoadCategory => ENEMIES")]
        public void LoadCategory()
        {
            GetFileDataHandler().LoadCategory(SaveCategory.Enemies);
        }

        [ContextMenu("LoadCategory => OverwriteValueInCategory")]
        public void LoadSingleCategory()
        {
            GetFileDataHandler().LoadSingleValueInCategory(
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

            GetFileDataHandler().OverwriteValueInCategory(
                SaveCategory.Player,
                "bdcc17d6-ccaf-4996-a0a9-a02f35708659",
                "RehvidGames.Entity.Health",
                obj
            );
        }

        [ContextMenu("SaveCategory")]
        public void SaveCat()
        {
            GetFileDataHandler().SaveCategory(SaveCategory.Enemies);
        }
    }
}
