namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEngine;

    public class Mana: MonoBehaviour, ISaveable 
    {
        [SerializeField] private float current = 100;
        [SerializeField] private float max = 100;


        public object Save()
        {
            return new ManaSaveData()
            {
                CurrentMana = current,
                MaxMana = max
            };
        }

        public void Load(object state)
        {
            ManaSaveData saveData = SaveManager.Instance.Serializer.Deserialize<ManaSaveData>(state);
            
            if (saveData == null)
            {
                Debug.LogWarning("Loading Mana system failed.");
                return;
            }
            
            current = saveData.CurrentMana;
            max = saveData.MaxMana;
        }
    }
    
    public class ManaSaveData
    {
        public float CurrentMana;
        public float MaxMana;
    }
}