namespace RehvidGames.Entity
{
    using NewSystem;
    using SaveSystem;
    using UnityEngine;

    public class Audio: MonoBehaviour, ISaveable
    {
        [SerializeField] private float volume;
        
        public object Save()
        {
            return volume;
        }

        public void Load(object state)
        {
            // var Volume = (float)state;
            // volume = Volume;
        }
    }
}