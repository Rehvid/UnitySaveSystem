namespace RehvidGames.Entity
{
    using SaveSystem;
    using UnityEngine;

    public class Audio: MonoBehaviour
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