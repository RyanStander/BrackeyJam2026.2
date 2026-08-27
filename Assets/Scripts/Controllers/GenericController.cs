using System;
using Combat.Stats;
using Events;
using Factories;
using UnityEngine;

namespace Controllers
{
    public class GenericController : MonoBehaviour
    {
        [SerializeField] public Health Health;

        protected virtual void OnValidate()
        {
            if (Health == null)
                Health = GetComponent<Health>();
        }

        protected virtual void Awake()
        {
            Health.OnDeath += HandleDeath;
        }
        
        protected virtual void HandleDeath()
        {
        }
    }
}
