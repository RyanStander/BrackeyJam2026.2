using System;
using Combat.Data;
using Combat.Rules;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    [RequireComponent(typeof(Rigidbody))]
    public class Throwable : MonoBehaviour
    {
        [SerializeField] private Rigidbody throwableRigidbody;
        private float weaponDamage;
        [SerializeField] private AIController instigator;
        private bool hitSomething = false;
        private bool returned = false;
        private Vector3 originalLocalPosition;

        private void OnValidate()
        {
            if (throwableRigidbody == null)
                throwableRigidbody = GetComponent<Rigidbody>();

            if (instigator == null)
                instigator = GetComponentInParent<AIController>();
        }
        
        private void Awake()
        {
            originalLocalPosition = transform.localPosition;
        }

        public void SetValues(float damage)
        {
            weaponDamage = damage;
        }

        public void Throw(Vector3 position, float speed)
        {
            Vector3 direction = (position - transform.position).normalized;
            throwableRigidbody.velocity = direction.normalized * speed;
            
            if (Vector3.Distance(transform.position, position) < 0.05f)
            {
                throwableRigidbody.velocity = Vector3.zero;
                hitSomething = true;
            }
        }

        public void Reset()
        {
            throwableRigidbody.velocity = Vector3.zero;
            hitSomething = false;
            returned = false;
            transform.localPosition = originalLocalPosition;
        }

        public bool HitSomething() => hitSomething;
        
        public bool Returned() => returned;

        public void Return(Vector3 position, float speed)
        {
            Vector3 direction = (position - transform.position).normalized;
            throwableRigidbody.velocity = direction * speed;
            
            if (Vector3.Distance(transform.position, position) < 0.05f)
            {
                throwableRigidbody.velocity = Vector3.zero;
                returned = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                hitSomething = true;

                DamageInfo damageInfo = new(
                    amount: weaponDamage,
                    sourceFaction: Faction.Enemies,
                    instigator: instigator.gameObject,
                    mode: DamageMode.Normal
                );

                if (other.gameObject.TryGetComponent(out IDamageable target))
                    if (CombatRules.CanDamage(damageInfo, target))
                        target.TakeDamage(damageInfo);
            }
            else if (!other.gameObject.CompareTag("Enemy") && other.gameObject.layer != LayerMask.NameToLayer("Floor"))
            {
                hitSomething = true;
            }
        }
    }
}
