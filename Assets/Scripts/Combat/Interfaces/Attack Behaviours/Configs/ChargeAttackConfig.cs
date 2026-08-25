using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "ChargeAttack", menuName = "Attacks", order = 0)]
    public class ChargeAttackConfig : ScriptableObject
    {
        public float AttackDistance = 5;
        public float WindupTime = 1;
        public float ChargeDistance = 7;
        public float ChargeSpeed = 1;
        public bool StunSelfOnObstacleHit = true;
        public float StunDuration = 1;
        public bool AllowsExploit = true;
        public float Cooldown = 5;
    }
}
