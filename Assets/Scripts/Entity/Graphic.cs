namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEditor.Overlays;
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
            GraphicSaveData saveData = GraphicSaveData.Load(state);
            if (saveData == null) return;
            
            useFullScreen = saveData.UseFullScreen;
        }
    }

    public class GraphicSaveData: BaseSaveData<GraphicSaveData>
    {
        public bool UseFullScreen;
    }
}