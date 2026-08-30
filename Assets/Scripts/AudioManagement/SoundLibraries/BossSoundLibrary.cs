using FMODUnity;
using UnityEngine;

namespace AudioManagement.SoundLibraries
{
    [CreateAssetMenu(fileName = "BossSoundLibrary", menuName = "Audio/Boss Sounds", order = 2)]
    public class BossSoundLibrary : ScriptableObject
    {
        public EventReference BossFlight;
        public EventReference ChargeFlight;
        public EventReference FlightPunchCombo;
        public EventReference Punch;
        public EventReference Slam;
    }
}
