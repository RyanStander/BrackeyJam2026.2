using System.Collections.Generic;
using Combat.Data;
using Combat.Interfaces;
using Combat.Rules;
using Combat.Stats;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Collider))]
    public class MeleeHitbox : MonoBehaviour
    {
        [SerializeField] private LayerMask hittableLayerMask;

        private readonly HashSet<Collider> hitThisSwing = new();
        private int damage;
        private GameObject instigator;

        public void BeginSwing(int swingDamage, GameObject swingInstigator)
        {
            damage = swingDamage;
            instigator = swingInstigator;
            hitThisSwing.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hitThisSwing.Contains(other)) return;
            if ((hittableLayerMask.value & (1 << other.gameObject.layer)) == 0) return;
            if (!other.gameObject.CompareTag("Enemy")) return;

            hitThisSwing.Add(other);

            if (other.TryGetComponent(out IDamageable target))
            {
                DamageInfo damageInfo = new(damage, Faction.Allies, instigator, DamageMode.Normal);
                if (CombatRules.CanDamage(damageInfo, target))
                    target.TakeDamage(damageInfo);
            }
        }
    }
}
