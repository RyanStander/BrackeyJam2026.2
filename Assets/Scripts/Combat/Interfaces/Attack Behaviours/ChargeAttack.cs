using System;
using System.Numerics;
using AudioManagement;
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

        private enum EnemyType
        {
            Charger,
            HandCow
        }

        [SerializeField] private EnemyType enemyType;

        public bool CanExecute(AIController controller) =>
            Vector3.Distance(controller.transform.position,
                controller.Target.transform.position) <=
            config.AttackDistance;

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            controller.AnimationController.PauseOnCurrentFrame();
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (phase == Phase.Windup && timer >= config.WindupTime)
            {
                switch (enemyType)
                {
                    case EnemyType.Charger:
                        AudioManager.PlayOneShot(AudioDataHandler.Charger.Attack);
                        break;
                    case EnemyType.HandCow:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                phase = Phase.Charging;
                timer = 0f;
                startLocation = controller.transform.position;
                targetDirection = (controller.Target.transform.position - startLocation).normalized;
                controller.AnimationController.PlayRun(targetDirection);
            }
            else if (phase == Phase.Charging)
            {
                Vector3 nextPosition = controller.transform.position +
                                       targetDirection * (config.ChargeSpeed * Time.deltaTime);

                controller.Movement.BeginManualOverride();
                controller.Movement.MovePosition(nextPosition);

                if (Vector3.Distance(startLocation, controller.transform.position) >= config.ChargeDistance)
                {
                    phase = Phase.Done;
                    controller.AnimationController.PauseOnCurrentFrame();
                }

                HandleHit(controller);
            }
        }

        private void HandleHit(AIController aiController)
        {
            Collider[] hits = Physics.OverlapSphere(aiController.transform.position, config.AttackSphereRadius,
                hittableLayerMask);

            foreach (Collider hit in hits)
            {
                if (aiController.IsHostileTo(hit.gameObject))
                {
                    phase = Phase.Done;
                    aiController.AnimationController.PauseOnCurrentFrame();

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
                        switch (enemyType)
                        {
                            case EnemyType.Charger:
                                AudioManager.PlayOneShot(AudioDataHandler.Charger.Exposed);
                                break;
                            case EnemyType.HandCow:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        
                        aiController.AnimationController.PlayStun(targetDirection);
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
