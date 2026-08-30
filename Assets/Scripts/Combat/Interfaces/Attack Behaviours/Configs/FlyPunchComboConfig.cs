using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "FlyPunchComboAttack", menuName = "Attacks/Fly Punch Combo", order = 0)]
    public class FlyPunchComboConfig : ScriptableObject
    {
        public float AttackDistance = 3.5f;
        public float WindupTime = 0.4f;
        public float HitInterval = 0.25f;
        public float RecoveryTime = 0.5f;
        public float HitRadius = 2f;
        public float DamagePerHit = 8f;
        public float Cooldown = 3f;
    }
}
