using System;
using Combat.Data;
using Combat.Interfaces;
using UnityEngine;

namespace Combat.Stats
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        public Faction Faction { get; private set; }
        public GameObject GameObject => gameObject;
        public event Action OnDeath;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
                OnDeath?.Invoke();
        }
    }
}
