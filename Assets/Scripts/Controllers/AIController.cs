using System;
using Combat.Stats;
using Movement;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(EnemyMovement),typeof(Health))]
    public class AIController : GenericController
    {
        
        [SerializeField] private EnemyMovement movement;
        public GameObject Target { get; set; }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (movement == null)
                movement = GetComponent<EnemyMovement>();
        }
    }
}
