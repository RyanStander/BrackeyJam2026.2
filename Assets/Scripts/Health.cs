    using System;
    using Combat.Interfaces;
    using UnityEngine;

    namespace Combat.Stats
    {
        public class Health : MonoBehaviour, IDamageable
        {
            [SerializeField] protected float MaxHealth = 100f;
            [SerializeField] protected float CurrentHealth;

            public GameObject GameObject => gameObject;
            public event Action OnDeath;
            protected bool isDead = false;

            public void Update()
            {
                if (!isDead && CurrentHealth <= 0)
                {
                    Die();
                }
            }
            protected virtual void Awake()
            {
                CurrentHealth = MaxHealth;
            }

            public virtual void TakeDamage(float damage)
            {
                if(isDead) return;
                CurrentHealth -= damage;
            }
            public virtual void Die()
            {
                if (isDead) return;        
                isDead = true;
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }