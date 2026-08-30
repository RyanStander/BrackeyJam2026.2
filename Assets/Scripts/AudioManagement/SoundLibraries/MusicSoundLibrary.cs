using FMODUnity;
using UnityEngine;

namespace AudioManagement.SoundLibraries
{
    [CreateAssetMenu(fileName = "MusicSoundLibrary", menuName = "Audio/Music Sounds", order = 2)]
    public class MusicSoundLibrary : ScriptableObject
    {
        public EventReference HUB;
        public EventReference BabyLevel;
        public EventReference NormalLevel;
        public EventReference BossLevel;
    }
}
