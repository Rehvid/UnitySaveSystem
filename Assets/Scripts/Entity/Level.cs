namespace RehvidGames.Entity
{
    using NewSystem;
    using SaveSystem;
    using UnityEngine;

    public class Level: MonoBehaviour, ISaveable
    {
        [SerializeField] private int level = 1;
        [SerializeField] private int experience = 25;
        
        public object Save()
        {
            return new LevelSystemData() {Level = level, Experience = experience};
        }

        public void Load(object state)
        {
          LevelSystemData data = JsonUtility.FromJson<LevelSystemData>(state.ToString());
          
          level = data.Level;
          experience = data.Experience;
        }
    }

    [System.Serializable]
    public struct LevelSystemData
    {
        public int Level;
        public int Experience;
    }
}