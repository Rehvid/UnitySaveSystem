namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEngine;

    public abstract class BaseSaveData<T> where T : class 
    {
        public static T Load(object state)
        {
            T data = SaveManager.Instance.Serializer.Deserialize<T>(state);
            if (data == null)
            {
                Debug.LogWarning($"Loading {typeof(T).Name} failed.");
            }
            return data;
        }
    }
}