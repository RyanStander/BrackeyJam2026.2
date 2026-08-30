using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(menuName = "Attacks/Punch Attack Config")]
    public class PunchAttackConfig : ScriptableObject
    {
        public float AttackDistance = 2f;
        public float WindupTime = 0.4f;
        public float Damage = 10f;
        public float Cooldown = 1.5f;
    }
}