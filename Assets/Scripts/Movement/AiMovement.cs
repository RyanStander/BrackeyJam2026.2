using System;
using UnityEngine;

namespace Movement
{
    public class AiMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody aiRigidbody;
        [SerializeField] private float speed = 1;
        [SerializeField] private bool goToPlayer = true;
        [SerializeField] private float range = 5;

        private void OnValidate()
        {
            if (aiRigidbody == null)
                aiRigidbody = GetComponent<Rigidbody>();
        }

        public void MovePosition(Vector3 nextPosition)
        {
            aiRigidbody.MovePosition(nextPosition);
        }

        public void MovementTick(Vector3 targetPosition)
        {
            if (goToPlayer)
            {
                MoveTowardsPlayer(targetPosition);
            }
            else
            {
                MoveInRangeOfPlayer(targetPosition);
            }
        }

        private void MoveTowardsPlayer(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 nextPosition = transform.position + direction * speed * Time.deltaTime;
            MovePosition(nextPosition);
        }

        private void MoveInRangeOfPlayer(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 nextPosition = transform.position + direction * speed * Time.deltaTime;

            if (Vector3.Distance(nextPosition, targetPosition) > range)
            {
                MovePosition(nextPosition);
            }
        }
    }
}
