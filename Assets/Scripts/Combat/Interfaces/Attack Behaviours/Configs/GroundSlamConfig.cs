using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "GroundSlamAttack", menuName = "Attacks/Ground Slam", order = 0)]
    public class GroundSlamConfig : ScriptableObject
    {
        public float AttackDistance = 5f;
        public float WindupTime = 0.8f;
        public float RecoveryTime = 0.6f;
        public float SlamRadius = 4f;
        public float Damage = 20f;
        public float KnockbackForce = 8f;
        public float Cooldown = 6f;
        public float SlamForwardOffset = 3f;
        public float SlamHeightOffset = 0f;
    }
}
