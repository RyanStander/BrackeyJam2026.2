using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "ThrowAttack", menuName = "Attacks/Throw", order = 1)]
    public class ThrowAttackConfig : ScriptableObject
    {
        public int Damage = 25;
        public float AttackDistance = 5;
        public float WindupTime = 1;
        public float ThrowSpeed = 1;
        public bool ReturnWeapon = true;
        public float ReturnSpeed = 1;
        public float Cooldown = 5;
    }
}
