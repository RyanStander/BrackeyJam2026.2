using Combat.Data;
using Combat.Rules;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.AttackExtensions
{
    public class Projectile : MonoBehaviour
    {
        private Transform target;
        private float damage;
        private AIController instigator;
        [SerializeField] private float speed = 15f;
        private Vector3 direction;
        private Faction sourceFaction;
        
        [SerializeField] private float maxLifetime = 5f;
        private float lifeTimer;

        public void Launch(Transform target, float damage, AIController instigator, float speed, Faction sourceFaction)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            this.speed = speed;
            direction = (target.position - transform.position).normalized;
            this.sourceFaction = sourceFaction;
        }

        void Update()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= maxLifetime) { Destroy(gameObject); return; }
            
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.position += direction * (speed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(instigator.TargetTag))
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    DamageInfo info = new(damage, sourceFaction, instigator.gameObject, DamageMode.Normal);
                    if (CombatRules.CanDamage(info, damageable))
                        damageable.TakeDamage(info);
                }

                Destroy(gameObject);
            }
        }
    }
}
