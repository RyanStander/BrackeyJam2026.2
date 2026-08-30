using AudioManagement;
using Combat.Data;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Combat.Rules;
using Combat.Stats;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class PunchAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private PunchConfig config;
        [SerializeField] private LayerMask hittableLayerMask;

        private enum Phase { Windup, Impact, Recovery, Done }
        private Phase phase;
        private float timer;

        public bool CanExecute(AIController controller) =>
            controller.Target != null &&
            Vector3.Distance(controller.transform.position, controller.Target.transform.position) <= config.AttackDistance;

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            controller.AnimationController.PlayPunch(DirectionToTarget(controller));
            AudioManager.PlayOneShot(AudioDataHandler.Boss.Punch);
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (phase == Phase.Windup && timer >= config.WindupTime)
            {
                phase = Phase.Impact;
                timer = 0f;
                HandleHit(controller);
            }
            else if (phase == Phase.Impact && timer >= config.RecoveryTime)
            {
                phase = Phase.Done;
            }
        }

        private void HandleHit(AIController controller)
        {
            Vector3 hitCenter = controller.transform.position + DirectionToTarget(controller) * (config.HitRadius * 0.5f);
            Collider[] hits = Physics.OverlapSphere(hitCenter, config.HitRadius, hittableLayerMask);

            foreach (Collider hit in hits)
            {
                if (!controller.IsHostileTo(hit.gameObject)) continue;

                DamageInfo info = new(config.Damage, controller.Faction, controller.gameObject, DamageMode.Normal);
                if (hit.gameObject.TryGetComponent(out IDamageable target) && CombatRules.CanDamage(info, target))
                    target.TakeDamage(info);

                return; // single-target punch
            }
        }

        public bool IsFinished(AIController controller) => phase == Phase.Done;
        public float Cooldown => config.Cooldown;

        private Vector3 DirectionToTarget(AIController c)
            => (c.Target.transform.position - c.transform.position).normalized;
    }
}
