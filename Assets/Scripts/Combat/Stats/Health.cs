using System;
using Combat.Data;
using Combat.Interfaces;
using Controllers;
using UnityEngine;

namespace Combat.Stats
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] public float MaxHealth = 100f;
        [SerializeField] public float CurrentHealth;

        public Faction Faction { get; private set; }
        public GameObject GameObject => gameObject;
        public event Action OnDeath;
        protected bool isDead = false;

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public virtual void TakeDamage(DamageInfo damageInfo)
        {
            if(isDead) return;
            CurrentHealth -= damageInfo.Amount;
            if (CurrentHealth <= 0)
                Die();
        }

        public virtual void Die()
        {
            if (isDead) return;
            isDead = true;
            OnDeath?.Invoke();
        }
    }
}
