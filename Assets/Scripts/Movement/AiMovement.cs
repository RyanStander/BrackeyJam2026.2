using System;
using UnityEngine;

namespace Movement
{
    public class AiMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody aiRigidbody;
        [SerializeField] public float speed = 1;
        [SerializeField] private float range = 5;

        private void OnValidate()
        {
            if (aiRigidbody == null)
                aiRigidbody = GetComponent<Rigidbody>();
        }

        public void MovePosition(Vector3 nextPosition)
        {
            nextPosition.y = aiRigidbody.position.y;
            aiRigidbody.MovePosition(nextPosition);
        }

        public void MovementTick(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 nextPosition = transform.position + direction * (speed * Time.deltaTime);
            nextPosition.y = aiRigidbody.position.y;

            if (Vector3.Distance(nextPosition, targetPosition) > range)
            {
                MovePosition(nextPosition);
            }
        }
    }
}
