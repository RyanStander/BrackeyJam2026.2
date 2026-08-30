using FMODUnity;
using UnityEngine;

namespace AudioManagement
{
    [CreateAssetMenu(fileName = "PlayerSoundLibrary", menuName = "Audio/Player Sounds")]
    public class PlayerSoundLibrary : ScriptableObject
    {
        public EventReference AttackHit;
        public EventReference AttackSwing;
        public EventReference Dodge;
        public EventReference Running;
        public EventReference Walking;
    }
}
