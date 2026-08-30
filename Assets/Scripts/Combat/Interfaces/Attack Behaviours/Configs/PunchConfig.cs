using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "PunchAttack", menuName = "Attacks/Punch", order = 0)]
    public class PunchConfig : ScriptableObject
    {
        public float AttackDistance = 3f;
        public float WindupTime = 0.5f;
        public float RecoveryTime = 0.4f;
        public float HitRadius = 2f;
        public float Damage = 15f;
        public float Cooldown = 2f;
    }
}
