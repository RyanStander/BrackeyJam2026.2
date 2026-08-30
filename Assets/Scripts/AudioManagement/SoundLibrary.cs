using AudioManagement.SoundLibraries;
using UnityEngine;

namespace AudioManagement
{
    [CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/Sound Library", order = 0)]
    public class SoundLibrary : ScriptableObject
    {
        public MusicSoundLibrary MusicSoundLibrary;
        public PlayerSoundLibrary PlayerSoundLibrary;
        public GunslingerSoundLibrary GunslingerSoundLibrary;
        public BossSoundLibrary BossSoundLibrary;
        public ChargerSoundLibrary ChargerSoundLibrary;
        public SwatterSoundLibrary SwatterSoundLibrary;
    }
}
