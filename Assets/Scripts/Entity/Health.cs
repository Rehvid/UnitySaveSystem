namespace RehvidGames.Entity
{
    
    using SaveSystem;
    using UnityEngine;

    public class Health: MonoBehaviour, ISaveable
    {
        [SerializeField] private float currentHealth = 1;
        [SerializeField] private float maxHealth = 100;
        
        
        public object Save()
        {
            return new HealthSystemData()
            {
                CurrentHealth = currentHealth, 
                MaxHealth = maxHealth, 
            };
        }

        public void Load(object state)
        {
            HealthSystemData systemData = JsonUtility.FromJson<HealthSystemData>(state.ToString());
            
            currentHealth = systemData.CurrentHealth;
            maxHealth = systemData.MaxHealth;
        }
    }

    [System.Serializable]
    public class HealthSystemData
    {
        public float CurrentHealth;
        public float MaxHealth;
    }
    
}