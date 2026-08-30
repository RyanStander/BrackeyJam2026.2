using Combat.Data;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Combat.Rules;
using Combat.Stats;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class FlyPunchComboAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private FlyPunchComboConfig config;
        [SerializeField] private LayerMask hittableLayerMask;

        private enum Phase { Windup, Hit1, Hit2, Hit3, Recovery, Done }
        private Phase phase;
        private float timer;

        public bool CanExecute(AIController controller) =>
            controller.Target != null &&
            Vector3.Distance(controller.transform.position, controller.Target.transform.position) <= config.AttackDistance;

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            controller.AnimationController.PlayFlyPunch(DirectionToTarget(controller));
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            switch (phase)
            {
                case Phase.Windup when timer >= config.WindupTime:
                    phase = Phase.Hit1;
                    timer = 0f;
                    HandleHit(controller);
                    break;

                case Phase.Hit1 when timer >= config.HitInterval:
                    phase = Phase.Hit2;
                    timer = 0f;
                    HandleHit(controller);
                    break;

                case Phase.Hit2 when timer >= config.HitInterval:
                    phase = Phase.Hit3;
                    timer = 0f;
                    HandleHit(controller);
                    break;

                case Phase.Hit3 when timer >= config.RecoveryTime:
                    phase = Phase.Done;
                    break;
            }
        }

        private void StartHit(AIController controller, Phase nextPhase, int variant)
        {
            phase = nextPhase;
            timer = 0f;
            controller.AnimationController.PlayFlyPunch(DirectionToTarget(controller));
            HandleHit(controller);
        }

        private void HandleHit(AIController controller)
        {
            Vector3 hitCenter = controller.transform.position + DirectionToTarget(controller) * (config.HitRadius * 0.5f);
            Collider[] hits = Physics.OverlapSphere(hitCenter, config.HitRadius, hittableLayerMask);

            foreach (Collider hit in hits)
            {
                if (!controller.IsHostileTo(hit.gameObject)) continue;

                DamageInfo info = new(config.DamagePerHit, controller.Faction, controller.gameObject, DamageMode.Normal);
                if (hit.gameObject.TryGetComponent(out IDamageable target) && CombatRules.CanDamage(info, target))
                    target.TakeDamage(info);

                return;
            }
        }

        public bool IsFinished(AIController controller) => phase == Phase.Done;
        public float Cooldown => config.Cooldown;

        private Vector3 DirectionToTarget(AIController c)
            => (c.Target.transform.position - c.transform.position).normalized;
    }
}
