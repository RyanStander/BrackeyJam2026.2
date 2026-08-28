using Combat.Data;
using Combat.Interfaces;
using Combat.Rules;
using UnityEngine;

namespace Controllers
{
    public class BossController : AIController
    {
        public int Phase { get; private set; } = 0;
        [SerializeField] private float phase2Threshold = 15f;

        private void LateUpdate()
        {
            if (Phase == 0 && Health.CurrentHealth <= phase2Threshold)
            {
                Phase = 1;
                Animator.SetTrigger("GrowWings");
            }
            
        }
        public bool HitTarget(float dmg, float range)
        {
            if (Target == null) 
            {
                return false;
            }
            if (Vector3.Distance(transform.position, Target.transform.position) > range) 
            {
                return false;
            }
            var damageable = Target.GetComponent<IDamageable>();

            var info = new DamageInfo(dmg, Faction, gameObject);

            if (!CombatRules.CanDamage(info, damageable)) 
            {
                return false;
            }
            damageable.TakeDamage(info);
            return true;
        }
        public void HitAoe(float dmg, float radius)
        {
            foreach (var colliders in Physics.OverlapSphere(transform.position, radius))
            {
                var damageable = colliders.GetComponent<IDamageable>();

                if (damageable == null) continue;

                var info = new DamageInfo(dmg, Faction, gameObject);

                if (CombatRules.CanDamage(info, damageable)) 
                {
                    damageable.TakeDamage(info);
                }
            }
        }
    }
}