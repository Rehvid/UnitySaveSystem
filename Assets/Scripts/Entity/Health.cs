namespace RehvidGames.Entity
{
    using Newtonsoft.Json;
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
            var systemData = JsonConvert.DeserializeObject<HealthSystemData>(state.ToString());
            
            Debug.Log(systemData.CurrentHealth);
            currentHealth = systemData.CurrentHealth;
            maxHealth = systemData.MaxHealth;
        }
    }
    
    
    [System.Serializable]
    public struct HealthSystemData
    {
        public float CurrentHealth;
        public float MaxHealth;
    }
}