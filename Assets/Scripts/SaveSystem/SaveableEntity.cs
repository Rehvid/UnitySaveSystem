namespace RehvidGames.NewSystem
{
    using System;
    using System.Collections.Generic;
    using Config;
    using PersistenceData;
    using SaveSystem;
    using UnityEngine;

    public class SaveableEntity: MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; } = string.Empty;
        [field: SerializeField] public SaveCategory Category { get; private set; }
        
        [ContextMenu("Generate Id")]
        private void GenerateId() => Id = Guid.NewGuid().ToString();

        public Dictionary<string, object> CaptureSaveableObjects()
        {
            var state = new Dictionary<string, object>();

            foreach (var saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.Save();
            }
            
            return state;
        }

        public void RestoreSaveableObjects(Dictionary<string, object> state)
        {
            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();
                if (state.TryGetValue(typeName, out object stateObject))
                {
                    saveable.Load(stateObject);
                }
            }
        }

        public void RestoreSingleSaveableObject(PersistedEntity entity)
        {
            foreach (var saveable in GetComponents<ISaveable>())
            {
                
                if (saveable.GetType().ToString() == entity.Type)
                {
                    Debug.Log(entity.SerializedContent);
                    saveable.Load(entity.SerializedContent);
                    break;
                }
            }
        }
        
    }
}