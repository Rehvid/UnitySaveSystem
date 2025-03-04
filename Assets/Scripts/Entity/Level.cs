namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEngine;

    public class Level: MonoBehaviour, ISaveable
    {
        [SerializeField] private int level = 1;
        [SerializeField] private int experience = 25;
        
        public object Save()
        {
            return new LevelSaveData() {Level = level, Experience = experience};
        }

        public void Load(object state)
        {
          LevelSaveData saveData = SaveManager.Instance.Serializer.Deserialize<LevelSaveData>(state);
            
          if (saveData == null)
          {
              Debug.LogWarning("Loading health system failed.");
              return;
          }
          
          level = saveData.Level;
          experience = saveData.Experience;
        }
    }

    [System.Serializable]
    public class LevelSaveData
    {
        public int Level;
        public int Experience;
    }
}