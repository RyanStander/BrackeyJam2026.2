using Combat.Data;
using Combat.Interfaces;
using Combat.Rules;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class BossCharge : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private float attackDistance = 30f;
        [SerializeField] private float windupTime = 0.6f;
        [SerializeField] private float chargeSpeed = 20f;
        [SerializeField] private float chargeDistance = 15f;
        [SerializeField] private float hitRadius = 1.5f;
        [SerializeField] private float chargeDamage = 30f;
        [SerializeField] private float cooldown = 2f;

        private enum Phase { Windup, Charging, Done }
        private Phase phase;
        private float timer;
        private Vector3 startLocation;
        private Vector3 direction;

        public bool CanExecute(AIController controller)
        {
            if (controller.Target == null) 
            { 
                return false; 
            
            }
            float distance = Vector3.Distance(controller.transform.position, controller.Target.transform.position);
            return distance <= attackDistance;
        }
        public void Telegraph(AIController controller)
        {
            phase = Phase.Windup;
            timer = 0f;
            controller.Animator.SetBool("isBusy", true); 
            controller.Animator.SetTrigger("ChargeWindup");
        }
    
        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (phase == Phase.Windup)
            {
                if (timer < windupTime) return;

                phase = Phase.Charging;
                startLocation = controller.transform.position;
                direction = (controller.Target.transform.position - startLocation).normalized;
                controller.Animator.SetTrigger("Charge");
                controller.Animator.SetBool("isBusy", true);
            }
            else if (phase == Phase.Charging)
            {
                var rb = controller.GetComponent<Rigidbody>();
                rb.velocity = new Vector3(direction.x * chargeSpeed, rb.velocity.y, direction.z * chargeSpeed);

                if (controller.Target != null &&
                    Vector3.Distance(controller.transform.position,
                        controller.Target.transform.position) <= hitRadius &&
                    controller.Target.TryGetComponent(out IDamageable target))
                {
                    var info = new DamageInfo(chargeDamage, controller.Faction, controller.gameObject);
                    if (CombatRules.CanDamage(info, target))
                        target.TakeDamage(info);

                    StopCharge(rb);
                    controller.Animator.SetBool("isBusy", false);
                    phase = Phase.Done;
                    return;
                }

                if (Vector3.Distance(startLocation, controller.transform.position) >= chargeDistance || timer >= 3f)
                {
                    StopCharge(rb);
                    controller.Animator.SetBool("isBusy", false);
                    phase = Phase.Done;
                }
            }
        }

        private void StopCharge(Rigidbody rb) => rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        public bool IsFinished(AIController controller) => phase == Phase.Done;
        public float Cooldown => cooldown;
    }
}