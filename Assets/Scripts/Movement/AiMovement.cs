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
        [SerializeField] private float animSpeedModifier = 1f;
        [SerializeField] private AIAnimationController animationController;

        private Vector3 smoothedMoveDirection = Vector3.forward;
        [SerializeField] private float directionSmoothing = 8f;
        
        private bool isOverridden;
        private bool attackControlsMovement;

        public bool IsOverridden => isOverridden;

        private void OnValidate()
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (animationController == null) animationController = GetComponent<AIAnimationController>();
        }

        private void Awake()
        {
            agent.speed = speed;
            agent.updateRotation = false;
            rb.isKinematic = true;
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

        public void ReleaseAttackMovementControl() => attackControlsMovement = false;

        public void MovePosition(Vector3 nextPosition)
        {
            if (isOverridden)
            {
                Vector3 targetDirection = (nextPosition - transform.position).normalized;
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
            if (isOverridden || attackControlsMovement) return;

            if (Vector3.Distance(transform.position, targetPosition) > range)
            {
                Vector3 offsetDirection = (transform.position - targetPosition).normalized;
                agent.SetDestination(targetPosition + offsetDirection * range);

                Vector3 rawDirection = agent.velocity.sqrMagnitude > 0.01f
                    ? agent.velocity.normalized
                    : smoothedMoveDirection;

                smoothedMoveDirection = Vector3.Slerp(smoothedMoveDirection, rawDirection, directionSmoothing * Time.deltaTime);

                if (animationController != null)
                    animationController.PlayRun(smoothedMoveDirection, animSpeedModifier);
            }
            else
            {
                agent.ResetPath();

                if (animationController != null)
                {
                    Vector3 facingDirection = targetPosition - transform.position;
                    if (facingDirection.sqrMagnitude > 0.001f)
                    {
                        smoothedMoveDirection = Vector3.Slerp(smoothedMoveDirection, facingDirection.normalized, directionSmoothing * Time.deltaTime);
                    }
                    animationController.PlayRun(smoothedMoveDirection, animSpeedModifier);
                }
            }
        }
        
        public void TickFleeAnimation()
        {
            if (animationController == null) return;

            Vector3 moveDirection = agent.velocity.sqrMagnitude > 0.01f
                ? agent.velocity.normalized
                : transform.forward;

            animationController.PlayRun(moveDirection, animSpeedModifier);
        }

        public bool SetFleeDestination(Vector3 threatPosition, float fleeDistance)
        {
            Vector3 awayDirection = transform.position - threatPosition;
            awayDirection.y = 0f;
            awayDirection.Normalize();

            Vector3 desiredPoint = transform.position + awayDirection * fleeDistance;

            if (!NavMesh.SamplePosition(desiredPoint, out NavMeshHit hit, 3f, NavMesh.AllAreas))
                return false;

            agent.SetDestination(hit.position);
            attackControlsMovement = true;
            return true;
        }

        public bool HasReachedDestination()
        {
            if (agent.pathPending) return false;
            if (!agent.hasPath) return false;
            if (agent.pathStatus != NavMeshPathStatus.PathComplete) return false;
            if (float.IsPositiveInfinity(agent.remainingDistance)) return false;

            return agent.remainingDistance <= agent.stoppingDistance;
        }
    }
}
