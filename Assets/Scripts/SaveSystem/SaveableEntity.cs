namespace RehvidGames.SaveSystem
{
    using System;
    using System.Collections.Generic;
    using Config;
    using PersistenceData;
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
            var components = GetComponents<ISaveable>();
           
            foreach (var saveable in components)
            {
                state[saveable.GetType().ToString()] = saveable.Save();
            }
            
            return state;
        }

        public void RestoreSaveableObjects(Dictionary<string, object> state)
        {
           ProcessSaveableComponents((saveable, entityTypeName) =>
           {
               if (state.TryGetValue(entityTypeName, out object stateObject))
               {
                   saveable.Load(stateObject);
               }

               return false;
           });
        }

        public void RestoreSingleSaveableObject(PersistedEntity entity)
        {
            ProcessSaveableComponents((saveable, entityTypeName) => {
                if (entityTypeName != entity.EntityType) return false;
                
                saveable.Load(entity.Data);
                return true;
            });
        }


        private void ProcessSaveableComponents(Func<ISaveable, string, bool> callback)
        {
            var components = GetComponents<ISaveable>();
            
            foreach (var saveable in components)
            {
                string typeName = saveable.GetType().ToString();
                
                if (callback(saveable, typeName))
                {
                    break; 
                }
            }
        }
    }
}