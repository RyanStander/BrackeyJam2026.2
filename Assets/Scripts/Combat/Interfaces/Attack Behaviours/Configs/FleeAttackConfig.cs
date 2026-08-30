using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "FleeAttack", menuName = "Attacks/Flee", order = 0)]
    public class FleeAttackConfig : ScriptableObject
    {
        public float TooCloseDistance = 3f;
        public float FleeDistance = 4f;
        public float MaxFleeDuration = 2f;
        public float Cooldown = 2f;
    }
}
