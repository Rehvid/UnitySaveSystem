namespace RehvidGames.Ui
{
    using Enums;
    using Record;
    using SaveSystem;
    using UnityEngine;

    public class SaveMenu: MonoBehaviour
    {
        [SerializeField] private SaveFileCategory category;
        
        [Header("Record Settings")]
        [SerializeField] private string recordId;
        [SerializeField] private SaveFileCategory recordCategory;
        
        [SerializeField, Tooltip("The class must implement interface ISaveable, otherwise this object won't be serialized properly.")] 
        private MonoBehaviour entityToSerialize;
        
        private SaveManager saveManager;

        private void Awake()
        {
            saveManager = SaveManager.Instance;
        }
        
        public void LoadSelectedCategory()
        {
            saveManager.LoadCategory(category);
        }

        public void SaveSelectedCategory()
        {
            saveManager.SaveCategory(category);
        }

        public void SaveSelectedRecord()
        {
            saveManager.SaveRecord(CreateSaveRecord());
        }
        
        public void LoadSelectedRecord()
        {
            Debug.LogWarning("Data for chosen entity to serialize won't be loaded, only type from object is taken there.");
            Debug.Log("Loading selected record...");
            saveManager.LoadRecord(CreateSaveRecord());
        }

        private SaveRecord CreateSaveRecord()
        {
            object data = null;
            
            if (entityToSerialize is ISaveable saveable)
            {
                data = saveable.Save();
            }
            else
            {
                Debug.LogError("Entity To Serialize is not implementing ISaveable");
            }
            
            return new SaveRecord(
                recordCategory,
                recordId,
                entityToSerialize.GetType().ToString(),
                data
            );
        }
    }
}