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
            AudioSaveData audioSaveData = SaveManager.Instance.Serializer.Deserialize<AudioSaveData>(state);
            
            if (audioSaveData == null)
            {
                Debug.LogWarning("Loading Audio Save Data failed.");
                return;
            }
            
            volume = audioSaveData.Volume;
            maxVolume = audioSaveData.MaxVolume;
        }
    }

    [System.Serializable]
    public class AudioSaveData
    {
        public float Volume;
        public float MaxVolume;
    }
}