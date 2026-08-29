using Combat.Animations;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class AiMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Rigidbody rb;
        [SerializeField] public float speed = 1;
        [SerializeField] private float range = 5;

        [SerializeField] private AIAnimationController animationController;

        private bool isOverridden;

        private void OnValidate()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (animationController == null) animationController = GetComponent<AIAnimationController>();
        }

        private void Awake()
        {
            agent.speed = speed;
            rb.isKinematic = true;
            agent.updateRotation = false;
        }

        public void BeginManualOverride()
        {
            if (isOverridden) return;
            isOverridden = true;
            agent.enabled = false;
            rb.isKinematic = false;
        }

        public void EndManualOverride()
        {
            if (!isOverridden) return;
            isOverridden = false;
            rb.isKinematic = true;
            agent.enabled = true;
            agent.Warp(transform.position);
        }

        public bool IsOverridden => isOverridden;

        public void MovePosition(Vector3 nextPosition)
        {
            Vector3 targetDirection = (nextPosition - transform.position).normalized;
            if (isOverridden)
            {
                if (animationController != null)
                    animationController.PlayRun(targetDirection);
                rb.MovePosition(nextPosition);
            }
            else
            {
                agent.Warp(nextPosition);
            }
        }

        public void MovementTick(Vector3 targetPosition)
        {
            if (isOverridden) return;

            float currentDistance = Vector3.Distance(transform.position, targetPosition);
            if (currentDistance > range)
            {
                Vector3 destination = targetPosition;

                if (range > 0f)
                {
                    // stop at preferredRange away from the target, not on top of it
                    Vector3 direction = (transform.position - targetPosition).normalized;
                    destination = targetPosition + direction * range;
                }

                agent.SetDestination(destination);
                Vector3 moveDirection = agent.velocity.sqrMagnitude > 0.01f ? agent.velocity.normalized : transform.forward;
                if (animationController != null)
                    animationController.PlayRun(moveDirection);
            }
            else
            {
                agent.ResetPath();
                animationController.PauseOnCurrentFrame();
            }
        }
    }
}
