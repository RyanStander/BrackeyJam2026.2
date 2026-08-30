using Combat.Boss;
using Combat.Data;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Combat.Rules;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class FlyChargeAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private FlyChargeConfig config;
        [SerializeField] private BossTelegraph telegraph;
        [SerializeField] private LayerMask hittableLayerMask;

        private enum Phase
        {
            Windup,
            Charging,
            Done
        }

        private Phase phase;
        private float timer;
        private Vector3 chargeDirection;
        private Vector3 startLocation;
        private bool hasHit;

        public bool CanExecute(AIController controller)
        {
            if (controller.Target == null) return false;

            Vector3 dir = controller.Target.transform.position - controller.transform.position;
            int index = GetDirectionIndex(dir);
            bool isSideFacing = index is 2 or 6; // Front_FlyCharge is broken - side only

            return isSideFacing && dir.magnitude <= config.AttackDistance;
        }

        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            hasHit = false;
            chargeDirection = (controller.Target.transform.position - controller.transform.position).normalized;
            controller.AnimationController.PlayFlyCharge(chargeDirection);
            telegraph.ShowLine(controller.transform.position, chargeDirection, config.ChargeDistance,
                config.TelegraphWidth, config.WindupTime);
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (phase == Phase.Windup && timer >= config.WindupTime)
            {
                phase = Phase.Charging;
                timer = 0f;
                startLocation = controller.transform.position;
                controller.Movement.BeginManualOverride();
            }
            else if (phase == Phase.Charging)
            {
                Vector3 next = controller.transform.position + chargeDirection * (config.ChargeSpeed * Time.deltaTime);
                controller.Movement.MovePosition(next);

                HandleChargeHit(controller);

                if (Vector3.Distance(startLocation, controller.transform.position) >= config.ChargeDistance)
                    phase = Phase.Done;
            }
        }

        private void HandleChargeHit(AIController controller)
        {
            Collider[] hits = Physics.OverlapSphere(controller.transform.position, config.HitRadius, hittableLayerMask);

            foreach (Collider hit in hits)
            {
                if (LayerMask.LayerToName(hit.gameObject.layer) == "Obstacles")
                {
                    Destroy(hit.gameObject);
                    continue;
                }

                if (hasHit) continue;
                if (!controller.IsHostileTo(hit.gameObject)) continue;

                hasHit = true;

                DamageInfo info = new(config.Damage, controller.Faction, controller.gameObject, DamageMode.Normal);
                if (hit.gameObject.TryGetComponent(out IDamageable target) && CombatRules.CanDamage(info, target))
                    target.TakeDamage(info);

                KnockbackHelper.Apply(hit.gameObject, chargeDirection, config.KnockbackForce);
            }
        }

        public bool IsFinished(AIController controller) => phase == Phase.Done;
        public float Cooldown => config.Cooldown;

        private int GetDirectionIndex(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float snapped = Mathf.Round(angle / 45f) * 45f;
            return ((Mathf.RoundToInt(snapped / 45f) % 8) + 8) % 8;
        }
    }
}
