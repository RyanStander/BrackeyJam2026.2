using System;
using System.Numerics;
using Combat.Data;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Combat.Rules;
using Combat.Stats;
using Controllers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class ChargeAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private ChargeAttackConfig config;
        [SerializeField] private LayerMask hittableLayerMask;
        private AIController aiController;

        private enum Phase
        {
            Windup,
            Charging,
            Done
        }

        private Phase phase;
        private float timer;
        private Vector3 startLocation;
        private Vector3 targetDirection;

        private static readonly int windup = Animator.StringToHash("Windup");
        private static readonly int charge = Animator.StringToHash("Charge");
        private static readonly int end = Animator.StringToHash("End");
        private static readonly int stunned = Animator.StringToHash("Stunned");

        public bool CanExecute(AIController controller) =>
            Vector3.Distance(controller.transform.position,
                controller.Target.transform.position) <=
            config.AttackDistance;

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            controller.Animator.SetTrigger(windup);
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (phase == Phase.Windup && timer >= config.WindupTime)
            {
                phase = Phase.Charging;
                timer = 0f;
                controller.Animator.SetTrigger(charge);
                startLocation = controller.transform.position;
                targetDirection = (controller.Target.transform.position - startLocation).normalized;
            }
            else if (phase == Phase.Charging)
            {
                aiController = controller;
                Vector3 nextPosition = controller.transform.position +
                                       targetDirection * (config.ChargeSpeed * Time.deltaTime);
                
                controller.Movement.BeginManualOverride();
                controller.Movement.MovePosition(nextPosition);
                
                if (Vector3.Distance(startLocation, controller.transform.position) >= config.ChargeDistance)
                {
                    phase = Phase.Done;
                    controller.Animator.SetTrigger(end);
                }
                
                HandleHit();
            }
        }

        private void HandleHit()
        {
            Collider[] hits = Physics.OverlapSphere(aiController.transform.position, config.AttackSphereRadius, hittableLayerMask);

            foreach (Collider hit in hits)
            {
                if (aiController.IsHostileTo(hit.gameObject))
                {
                    phase = Phase.Done;
                    aiController.Animator.SetTrigger(end);

                    DamageInfo chargeInfo = new(
                        amount: config.Damage,
                        sourceFaction: Faction.Enemies,
                        instigator: aiController.gameObject,
                        mode: DamageMode.Normal
                    );

                    if (hit.gameObject.TryGetComponent(out IDamageable target))
                        if (CombatRules.CanDamage(chargeInfo, target))
                            target.TakeDamage(chargeInfo);
                }
                else if (!hit.gameObject.CompareTag(aiController.tag))
                {
                    if (config.StunSelfOnObstacleHit)
                    {
                        aiController.Animator.SetTrigger(stunned);
                        aiController.Stun(config.StunDuration);
                    }

                    phase = Phase.Done;
                }
            }
        }
        
        #if UNITY_EDITOR
        [SerializeField] private bool showGizmos = true;
        private void OnDrawGizmosSelected()
        {
            if (config == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, config.AttackSphereRadius);
        }
        #endif

        public bool IsFinished(AIController controller) => phase == Phase.Done;

        public float Cooldown => config.Cooldown;
    }
}
