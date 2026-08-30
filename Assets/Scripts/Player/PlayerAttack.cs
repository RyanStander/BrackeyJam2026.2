using System;
using AudioManagement;
using Combat.Data;
using Combat.Interfaces.Attack_Behaviours.AttackExtensions;
using Combat.Stats;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private int meleeAttackDamage = 10;
        [SerializeField] private float attackCooldown = 0.5f;

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
        
        [SerializeField] private WeaponSwingController weaponSwingController;
        [SerializeField] private MeleeHitbox meleeHitbox;
        private int currentDirectionIndex;

        private void OnValidate()
        {
            if (playerMovement == null)
                playerMovement = GetComponent<PlayerMovement>();
            if (weaponSwingController == null)
                weaponSwingController = GetComponent<WeaponSwingController>();
            if (meleeHitbox == null)
                GetComponentInChildren<MeleeHitbox>();
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

            currentDirectionIndex = directionIndex;
            
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

        private void MeleeAttack()
        {
            meleeHitbox.BeginSwing(meleeAttackDamage, gameObject);
            AudioManager.PlayOneShot(AudioDataHandler.Player.AttackSwing);
            weaponSwingController.PlaySwing(currentDirectionIndex, attackOffset);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
