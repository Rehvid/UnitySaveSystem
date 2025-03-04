namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEngine;

    public class Graphic : MonoBehaviour, ISaveable
    {
        [SerializeField] private bool useFullScreen = true;


        public object Save()
        {
            return new GraphicSaveData
            {
                UseFullScreen = useFullScreen
            };
        }

        public void Load(object state)
        {
            GraphicSaveData saveData = SaveManager.Instance.Serializer.Deserialize<GraphicSaveData>(state);
            
            if (saveData == null)
            {
                Debug.LogWarning("Loading health system failed.");
                return;
            }
            useFullScreen = saveData.UseFullScreen;
        }
    }

    public class GraphicSaveData
    {
        public bool UseFullScreen;
    }
}