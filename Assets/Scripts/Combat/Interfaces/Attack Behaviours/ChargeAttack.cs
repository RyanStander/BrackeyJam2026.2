using System.Numerics;
using Combat.Interfaces.Attack_Behaviours.Configs;
using Controllers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class ChargeAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField]private ChargeAttackConfig config;

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
                
                Vector3 nextPosition = controller.transform.position + targetDirection * config.ChargeSpeed * Time.deltaTime;
                controller.Movement.MovePosition(nextPosition);

                if (Vector3.Distance(startLocation, controller.transform.position) >= config.ChargeDistance)
                {
                    phase = Phase.Done;
                    controller.Animator.SetTrigger(end);
                }
            }
        }

        public bool IsFinished(AIController controller) => phase == Phase.Done;

        public float Cooldown => config.Cooldown;
    }
}
