using Combat.Interfaces.Attack_Behaviours.Configs;
using Controllers;
using UnityEngine;

namespace Combat.Interfaces.Attack_Behaviours
{
    public class FleeAttack : MonoBehaviour, IAttackBehaviour
    {
        [SerializeField] private FleeAttackConfig config;

        private float timer;

        public float Cooldown => config.Cooldown;

        public bool CanExecute(AIController controller) =>
            controller.Target != null &&
            Vector3.Distance(controller.transform.position, controller.Target.transform.position) <= config.TooCloseDistance;

        public void Telegraph(AIController controller)
        {
            timer = 0f;

            if (controller.Target != null)
                controller.Movement.SetFleeDestination(controller.Target.transform.position, config.FleeDistance);
        }

        public void Execute(AIController controller)
        {
            timer += Time.deltaTime;

            if (IsFinished(controller))
            {
                controller.Movement.ReleaseAttackMovementControl();
                controller.AnimationController.PauseOnCurrentFrame();
                return;
            }

            controller.Movement.TickFleeAnimation();
        }

        public bool IsFinished(AIController controller) =>
            controller.Movement.HasReachedDestination() || timer >= config.MaxFleeDuration;
    }
}
