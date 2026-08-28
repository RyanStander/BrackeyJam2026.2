using System;
using Combat.Data;
using Combat.Stats;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private int meleeAttackDamage = 10;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private float attackRadius = 1f;
        [SerializeField] private GameObject meleePrefab;

        [SerializeField] private PlayerMovement playerMovement;

        [Header("AttackDirectionOffsets")]
        [SerializeField] private Vector3 attackBackOffset = new (0f, 2f, 1.5f);
        [SerializeField] private Vector3 attackFrontOffset = new(0f, 2f, -1.5f);
        [SerializeField] private Vector3 attackLeftOffset = new (-1.5f, 2f, 0);
        [SerializeField] private Vector3 attackRightOffset = new(1.5f, 2f, 0);
        
        [SerializeField] private Vector3 attackUpRightOffset = new (1f, 2f, 1f);
        [SerializeField] private Vector3 attackDownRightOffset = new(1f, 2f, -1f);
        [SerializeField] private Vector3 attackUpLeftOffset = new (-1f, 2f, 1f);
        [SerializeField] private Vector3 attackDownLeftOffset = new(-1f, 2f, -1f);
        private Vector3 attackOffset;
        
        private float nextAttackTime;

        private void OnValidate()
        {
            if (playerMovement == null)
                playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            meleePrefab.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
            {
                MeleeAttack();
            }
        }

        public void SetAttackDirection(int directionIndex, float movementMagnitude)
        {
            if (movementMagnitude <= 0f)
                return;
            
            /*always y 2
            Forward attack is z1
            Back attack is z-1
            left attack is x1
            right attack is x-1
            */
            attackOffset = directionIndex switch
            {
                0 => attackBackOffset,
                1 => attackUpRightOffset,
                2 => attackRightOffset,
                3 => attackDownRightOffset,
                4 => attackFrontOffset,
                5 => attackUpLeftOffset,
                6 => attackLeftOffset,
                7 => attackDownLeftOffset,
                _ => attackOffset
            };
        }
        
        /// <summary>
        /// Converts an input vector into one of 8 cardinal direction indices (0-7),
        /// snapped to the nearest 45-degree increment.
        /// </summary>
        private int GetDirectionIndex(Vector2 input)
        {
            float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            float snappedAngle = Mathf.Round(angle / 45f) * 45f;
            int index = Mathf.RoundToInt(snappedAngle / 45f);
            return ((index % 8) + 8) % 8;
        }

        private void MeleeAttack()
        {
            Debug.Log("Melee attack performed!");
            DetectMeleeEnemies();
            //Lunge();
            nextAttackTime = Time.time + attackCooldown;
        }

        private void DetectMeleeEnemies()
        {
            if (attackPoint == null)
            {
                return;
            }

            Vector3 attackCenter = attackPoint.position + attackOffset;
            Collider[] hitEnemies = Physics.OverlapSphere(attackCenter, attackRadius);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    Debug.Log("Hit " + enemy.name);
                    enemy.GetComponent<Health>()
                        ?.TakeDamage(new DamageInfo(meleeAttackDamage, Faction.Enemies, gameObject));
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position + attackOffset, attackRadius);
            Gizmos.color = Color.white;
        }
    }
}
