using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.Configs
{
    [CreateAssetMenu(fileName = "ShootAttack", menuName = "Attacks/Shoot", order = 0)]
    public class ShootAttackConfig : ScriptableObject
    {
        public int Damage = 25;
        public float AttackDistance = 5;
        public GameObject ProjectilePrefab;
        public float WindupTime = 1;
        public float ProjectileSpeed = 1;
        public float ReloadTime = 2;
        public float Cooldown = 5;

        [Header("Spine Animation Names")] public string ShootFrontAnimationName;
        public string ShootBackAnimationName;
        public string ShootSideAnimationName;
        public string RunFrontAnimationName;
        public string RunBackAnimationName;
        public string RunSideAnimationName;
    }
}
