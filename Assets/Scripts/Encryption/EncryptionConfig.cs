namespace RehvidGames.Encryption
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "EncryptionConfig", menuName = "Encryption/EncryptionConfig")]
    public class EncryptionConfig : ScriptableObject
    {
        [SerializeField] private string encryptionKey = "1234567890123456"; 
        [SerializeField] private string encryptionIV = "6543210987654321"; 

        public string Key => encryptionKey;
        public string IV => encryptionIV;
        
        private const int KeyLength = 16;
        private const int IVLength = 16;
        
        private void OnValidate()
        {
            if (encryptionKey.Length != KeyLength)
            {
                Debug.LogError($"🔴 encryptionKey musi mieć dokładnie {KeyLength} znaków!");
                encryptionKey = encryptionKey.Length > KeyLength ? encryptionKey.Substring(0, KeyLength) : encryptionKey.PadRight(KeyLength, '0');
            }

            if (encryptionIV.Length != IVLength)
            {
                Debug.LogError($"🔴 encryptionIV musi mieć dokładnie {IVLength} znaków!");
                encryptionIV = encryptionIV.Length > IVLength ? encryptionIV.Substring(0, IVLength) : encryptionIV.PadRight(IVLength, '0');
            }
        }
    }
}