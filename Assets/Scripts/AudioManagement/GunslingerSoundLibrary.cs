using FMODUnity;
using UnityEngine;

namespace AudioManagement
{
    [CreateAssetMenu(fileName = "GunslingerSoundLibrary", menuName = "Audio/Gunslinger Sounds")]
    public class GunslingerSoundLibrary : ScriptableObject
    {
        public EventReference BasicShot;
        public EventReference Reload;
        public EventReference SpecialShot;
        public EventReference Walking;
    }
}
