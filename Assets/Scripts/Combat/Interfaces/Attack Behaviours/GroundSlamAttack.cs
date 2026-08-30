using AudioManagement;
using Combat.Boss;
using Combat.Data;
using Combat.Interfaces;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Combat.Rules;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class GroundSlamAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private GroundSlamConfig config;
        [SerializeField] private BossTelegraph telegraph;
        [SerializeField] private LayerMask hittableLayerMask;
        [SerializeField] private ParticleSystem spikeEffect;

        private enum Phase
        {
            Windup,
            Impact,
            Done
        }

        private Phase phase;
        private float timer;
        private Vector3 slamOrigin; // locked at telegraph time

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;

            Vector3 direction = DirectionToTarget(controller);
            slamOrigin = GetSlamOrigin(controller); // <-- computed and cached here

            controller.Movement.BeginManualOverride();
            controller.AnimationController.PlaySlam(direction);
            AudioManager.PlayOneShot(AudioDataHandler.Boss.Slam);
            telegraph.ShowCircle(slamOrigin, config.SlamRadius, config.WindupTime);
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (phase == Phase.Windup && timer >= config.WindupTime)
            {
                phase = Phase.Impact;
                timer = 0f;
                spikeEffect?.Play();
                HandleSlamHit(controller);
            }
            else if (phase == Phase.Impact && timer >= config.RecoveryTime)
            {
                phase = Phase.Done;
            }
        }

        private void HandleSlamHit(AIController controller)
        {
            Collider[] hits =
                Physics.OverlapSphere(slamOrigin, config.SlamRadius, hittableLayerMask); // <-- uses cached origin
            foreach (Collider hit in hits)
            {
                if (!controller.IsHostileTo(hit.gameObject)) continue;

                DamageInfo info = new(config.Damage, controller.Faction, controller.gameObject, DamageMode.Normal);
                if (hit.gameObject.TryGetComponent(out IDamageable target) && CombatRules.CanDamage(info, target))
                    target.TakeDamage(info);

                Vector3 outward = hit.transform.position - slamOrigin;
                KnockbackHelper.Apply(hit.gameObject, outward, config.KnockbackForce);
            }
        }

        private Vector3 GetSlamOrigin(AIController controller)
        {
            int index = GetDirectionIndex(DirectionToTarget(controller));

            Vector3 facing = index switch
            {
                0 or 1 or 7 => Vector3.forward,
                3 or 4 or 5 => Vector3.back,
                2 => Vector3.right,
                6 => Vector3.left,
                _ => Vector3.forward
            };

            return controller.transform.position
                   + facing * config.SlamForwardOffset
                   + Vector3.up * config.SlamHeightOffset;
        }

        private int GetDirectionIndex(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float snapped = Mathf.Round(angle / 45f) * 45f;
            return ((Mathf.RoundToInt(snapped / 45f) % 8) + 8) % 8;
        }

        private Vector3 DirectionToTarget(AIController c)
            => (c.Target.transform.position - c.transform.position).normalized;

        public bool CanExecute(AIController controller) =>
            controller.Target != null &&
            Vector3.Distance(controller.transform.position, controller.Target.transform.position) <=
            config.AttackDistance;

        public bool IsFinished(AIController controller) => phase == Phase.Done;
        public float Cooldown => config.Cooldown;
    }
}
