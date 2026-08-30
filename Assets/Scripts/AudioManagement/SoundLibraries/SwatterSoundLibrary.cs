using FMODUnity;
using UnityEngine;

namespace AudioManagement.SoundLibraries
{
    [CreateAssetMenu(fileName = "SwatterSoundLibrary", menuName = "Audio/Swatter Sounds", order = 2)]
    public class SwatterSoundLibrary : ScriptableObject
    {
        public EventReference Attack;
        public EventReference TakeDamage;
        public EventReference Walking;
    }
}
