using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "ChargeAttack", menuName = "Attacks", order = 0)]
    public class ChargeAttackConfig : ScriptableObject
    {
        public float AttackRange;
        public float WindupTime;
        public float ChargeDistance;
        public float ChargeSpeed;
        public bool StunSelfOnObstacleHit;
        public float StunDuration;
        public bool AllowsExploit;
    }
}
