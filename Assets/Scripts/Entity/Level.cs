namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEditor.Overlays;
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
            LevelSaveData saveData = LevelSaveData.Load(state);
            if (saveData == null) return;
          
            level = saveData.Level;
            experience = saveData.Experience;
        }
    }

    [System.Serializable]
    public class LevelSaveData: BaseSaveData<LevelSaveData>
    {
        public int Level;
        public int Experience;
    }
}