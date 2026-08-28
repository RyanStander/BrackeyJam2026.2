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

        protected virtual void Awake()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            if(isDead) return;
            currentHealth -= damageInfo.Amount;
            if (currentHealth <= 0)
                Die();
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

        public virtual void Die()
        {
            if (isDead) return;
            isDead = true;
            OnDeath?.Invoke();
        }
    }
}
