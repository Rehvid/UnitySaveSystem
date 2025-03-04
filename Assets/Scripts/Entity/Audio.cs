namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEngine;

    public class Audio: MonoBehaviour, ISaveable
    {
        [SerializeField] private float volume = 50f;
        [SerializeField] private float maxVolume = 100f;
        
        public object Save()
        {
            return new AudioSaveData()
            {
                Volume = volume,
                MaxVolume = maxVolume
            };
        }

        public void Load(object state)
        {
            AudioSaveData saveData = AudioSaveData.Load(state);
            if (saveData == null) return;
            
            volume = saveData.Volume;
            maxVolume = saveData.MaxVolume;
        }
    }

    [System.Serializable]
    public class AudioSaveData: BaseSaveData<AudioSaveData>
    {
        public float Volume;
        public float MaxVolume;
    }
}