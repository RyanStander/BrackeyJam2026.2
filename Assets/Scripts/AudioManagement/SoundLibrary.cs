using UnityEngine;

namespace AudioManagement
{
    [CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/Sound Library", order = 0)]
    public class SoundLibrary : ScriptableObject
    {
        public PlayerSoundLibrary PlayerSoundLibrary;
        public GunslingerSoundLibrary GunslingerSoundLibrary;
        public ChargerSoundLibrary ChargerSoundLibrary;
    }
}
