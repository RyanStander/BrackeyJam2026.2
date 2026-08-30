using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "FlyChargeAttack", menuName = "Attacks/Fly Charge", order = 0)]
    public class FlyChargeConfig : ScriptableObject
    {
        public float AttackDistance = 10f;
        public float WindupTime = 0.7f;
        public float ChargeSpeed = 14f;
        public float ChargeDistance = 12f;
        public float HitRadius = 2.5f;
        public float Damage = 25f;
        public float KnockbackForce = 12f;
        public float TelegraphWidth = 2.5f;
        public float Cooldown = 7f;
    }
}
