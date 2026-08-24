using System;
using Combat.Stats;
using UnityEngine;

namespace Controllers
{
    public class GenericController : MonoBehaviour
    {
        [SerializeField] protected Health Health;

        protected virtual void OnValidate()
        {
            if (Health == null)
                Health = GetComponent<Health>();
        }

        private void Awake()
        {
            Health.OnDeath += HandleDeath;
        }
        
        protected virtual void HandleDeath()
        {
            
        }
    }
}
