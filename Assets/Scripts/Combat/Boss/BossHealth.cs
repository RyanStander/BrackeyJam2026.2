using Combat.Data;
using Combat.Stats;
using UnityEngine;

namespace Combat.Boss
{
    public class BossHealth : Health
    {
        [SerializeField] private BossController bossController;

        public override void TakeDamage(DamageInfo damageInfo)
        {
            if (bossController != null && bossController.IsTransitioning)
                return; // invulnerable while transforming

            base.TakeDamage(damageInfo);
        }
    }
}
