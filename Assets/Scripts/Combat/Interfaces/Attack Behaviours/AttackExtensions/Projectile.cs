using Combat.Data;
using Combat.Rules;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours.AttackExtensions
{
    public class Projectile : MonoBehaviour
    {
        private GameObject target;
        private float damage;
        private AIController instigator;
        [SerializeField] private float speed = 15f;
        private Vector3 direction;
        private Faction sourceFaction;
        
        [SerializeField] private float maxLifetime = 5f;
        private float lifeTimer;
        private bool launched;
        
        [SerializeField] private Transform pivotTransform;
        
        public void Launch(GameObject targetGameObject, float damage, AIController instigator, float speed, Faction sourceFaction)
        {
            target = targetGameObject;
            this.damage = damage;
            this.instigator = instigator;
            this.speed = speed;
            direction = (GetAimPoint(target) - transform.position).normalized;
            this.sourceFaction = sourceFaction;
            launched = true;
            FaceTarget(direction);
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
            if (!launched || other==null) return;
            
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
        
        private void FaceTarget(Vector3 direction)
        {
            float angle = -Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            pivotTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        
        private Vector3 GetAimPoint(GameObject target)
        {
            if (target.TryGetComponent(out Collider col))
                return col.bounds.center;

            return target.transform.position; // fallback if no collider found
        }
    }
}
