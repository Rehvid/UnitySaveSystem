namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEngine;

    public class Health: MonoBehaviour, ISaveable
    {
        [SerializeField] private float currentHealth = 25;
        [SerializeField] private float maxHealth = 100;
        
        public object Save()
        {
            return new HealthSaveData()
            {
                CurrentHealth = currentHealth, 
                MaxHealth = maxHealth, 
            };
        }

        public void Load(object state)
        { 
            HealthSaveData saveData = HealthSaveData.Load(state);
            if (saveData == null) return;
            
            currentHealth = saveData.CurrentHealth;
            maxHealth = saveData.MaxHealth;
        }
    }
    
    
    [System.Serializable]
    public class HealthSaveData: BaseSaveData<HealthSaveData>
    {
        public float CurrentHealth;
        public float MaxHealth;
    }
}