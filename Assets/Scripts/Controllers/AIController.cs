using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Interfaces.Attack_Behaviours;
using Combat.Stats;
using Movement;
using Unity.VisualScripting;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(AiMovement), typeof(Health), typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class AIController : GenericController
    {
        private IAttackBehaviour[] attacks;
        private Dictionary<IAttackBehaviour, float> cooldownTimers = new();
        public AiMovement Movement;
        public Animator Animator;
        public GameObject Target { get; set; }
        private StateMachine.StateMachine stateMachine = new StateMachine.StateMachine();

        protected override void OnValidate()
        {
            base.OnValidate();

            if (Movement == null)
                Movement = GetComponent<AiMovement>();

            if (Animator == null)
                Animator = GetComponent<Animator>();
        }

        protected override void Awake()
        {
            base.Awake();
            stateMachine.Setup(this);
            attacks = GetComponentsInChildren<IAttackBehaviour>();
            
            Target = GameObject.FindGameObjectWithTag("Player");
        }
        
        private void Update()
        {
            stateMachine.Tick();
            UpdateCooldowns();
        }
        
        private void UpdateCooldowns()
        {
            List<IAttackBehaviour> keys = cooldownTimers.Keys.ToList();
            foreach (IAttackBehaviour attack in keys)
            {
                cooldownTimers[attack] -= Time.deltaTime;
                if (cooldownTimers[attack] <= 0)
                {
                    cooldownTimers.Remove(attack);
                }
            }
        }

        public IAttackBehaviour PickAvailableAttack()
        {
            List<IAttackBehaviour> candidates = attacks
                .Where(attack => attack.CanExecute(this) && !cooldownTimers.ContainsKey(attack)).ToList();

            if (candidates.Count == 0)
                return null;

            IAttackBehaviour chosenAttack = candidates[UnityEngine.Random.Range(0, candidates.Count)];
            cooldownTimers[chosenAttack] = chosenAttack.Cooldown;
            return chosenAttack;
        }
    }
}
