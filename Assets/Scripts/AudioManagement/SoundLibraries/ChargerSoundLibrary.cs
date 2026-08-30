using FMODUnity;
using UnityEngine;

namespace AudioManagement.SoundLibraries
{
    [CreateAssetMenu(fileName = "ChargerSoundLibrary", menuName = "Audio/Charger Sounds", order = 2)]
    public class ChargerSoundLibrary : ScriptableObject
    {
        public EventReference Attack;
        public EventReference Exposed;
        public EventReference Walking;
    }
}

