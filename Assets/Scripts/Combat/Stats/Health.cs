using System;
using Combat.Data;
using Combat.Interfaces;
using UnityEngine;

namespace Combat.Stats
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float currentHealth;

        public Faction Faction { get; private set; }
        public GameObject GameObject => gameObject;
        public event Action OnDeath;
        protected bool isDead = false;
        public GameObject LastKiller { get; private set; }

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            if (isDead) return;
            currentHealth -= damageInfo.Amount;
            if (currentHealth <= 0)
                Die(damageInfo.Instigator);
        }
        
        public virtual void RestoreHealth(float amount)
        {
            if(isDead) return;
            currentHealth += amount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
        
        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;

        public virtual void Die(GameObject killer = null)
        {
            if (isDead) return;
            isDead = true;
            LastKiller = killer;
            OnDeath?.Invoke();
        }
    }
}
