using System;
using Combat.Data;
using Combat.Interfaces;
using UnityEngine;

namespace Combat.Stats
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] protected float MaxHealth = 100f;
        protected float CurrentHealth;

        public Faction Faction { get; private set; }
        public GameObject GameObject => gameObject;
        public event Action OnDeath;

        protected virtual void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                OnDeath?.Invoke();
        }
    }
}
