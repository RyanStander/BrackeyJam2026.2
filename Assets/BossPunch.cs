using Combat.Data;
using Combat.Interfaces;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Combat.Rules;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class PunchAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private PunchAttackConfig config;

        private enum Phase { Punching, Done }
        private Phase phase;

        public bool CanExecute(AIController controller)
        {
            if (controller.Target == null || config == null) return false;
            return Vector3.Distance(controller.transform.position, controller.Target.transform.position) <= config.AttackDistance;
        }
        public void Telegraph(AIController controller)
        {
            phase = Phase.Punching;
            controller.Animator.SetTrigger("Punch");
        }

        public void Execute(AIController controller)
        {
            if (phase != Phase.Punching) return;

            if (controller.Target != null &&
                Vector3.Distance(controller.transform.position, controller.Target.transform.position) <= config.AttackDistance && controller.Target.TryGetComponent(out IDamageable target))
            {
                var info = new DamageInfo(config.Damage, controller.Faction, controller.gameObject);
                if (CombatRules.CanDamage(info, target))
                    target.TakeDamage(info);
            }

            phase = Phase.Done;
        }

        public bool IsFinished(AIController controller) => phase == Phase.Done;
        public float Cooldown => config.Cooldown;
    }
}