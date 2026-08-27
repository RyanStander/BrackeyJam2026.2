using System;
using System.Collections.Generic;
using System.Linq;
using Combat.Data;
using Combat.Interfaces.Attack_Behaviours;
using Combat.Stats;
using Events;
using Factories;
using Movement;
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
        private bool exploited;
        public bool WasExploited { get; private set; }
        public string TargetTag = "Player";
        public Faction Faction = Faction.Enemies;
        
        public static event Action<AIController, bool> OnEnemyDeath;
        public bool IsEnemyFaction => TargetTag == "Player";

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

            ReacquireTarget();
            Health.OnDeath += OnDeath;
            Health.OnDeath += HandleOwnDeath;
        }

        public void ReacquireTarget()
        {
            if (TargetTag == "Enemy")
            {
                Target = FindNearest(GameObject.FindGameObjectsWithTag("Enemy"));
            }
            else if (TargetTag == "Player")
            {
                Target = FindNearest(
                    GameObject.FindGameObjectsWithTag("Player")
                        .Concat(GameObject.FindGameObjectsWithTag("Companion"))
                );
            }
            else
            {
                Target = GameObject.FindGameObjectWithTag(TargetTag);
            }
        }

        private GameObject FindNearest(IEnumerable<GameObject> candidates)
        {
            return candidates
                .Where(t => t != null && t != gameObject)
                .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
                .FirstOrDefault();
        }

        protected virtual void Update()
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

        private void OnDeath()
        {
            EventManager.currentManager.AddEvent(new OnEnemyDeath());

            Debug.Log($"{gameObject.name} has died.");
            EventManager.currentManager.AddEvent(new CreatePickup(PickupType.Scrap, transform.position));
            Destroy(gameObject);
        }
        
        private void HandleOwnDeath()
        {
            if (IsEnemyFaction)
                OnEnemyDeath?.Invoke(this, WasExploited);
        }

        public void Stun(float duration)
        {
            stateMachine.Stun(duration);
            exploited = true;
        }

        //for damage bonus on exploited enemies, should only happen once
        public bool IsExploitable()
        {
            if (!exploited)
                return false;

            exploited = false;
            WasExploited = true;
            return true;
        }
        
        public enum BetrayalType { Hostile, StealLoot }

        public void TriggerBetrayal(BetrayalType type)
        {
            switch (type)
            {
                case BetrayalType.Hostile:
                    TargetTag = "Player";
                    gameObject.tag = "Enemy";
                    Target = GameObject.FindGameObjectWithTag("Player");
                    break;

                case BetrayalType.StealLoot:
                    Debug.LogWarning($"{gameObject.name}: StealLoot betrayal not yet implemented, falling back to Hostile.");
                    goto case BetrayalType.Hostile;
            }
        }
    }
}
