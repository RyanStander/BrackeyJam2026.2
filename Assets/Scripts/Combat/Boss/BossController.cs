using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AudioManagement;
using Combat.Interfaces.Attack_Behaviours;
using Controllers;
using UnityEngine;

namespace Combat.Boss
{
    public class BossController : AIController
    {
        [Header("Phase")]
        [SerializeField] [Range(0f, 1f)] private float phase2HealthPercent = 0.5f;
        [SerializeField] private float phase2SpeedMultiplier = 1.5f;

        [Header("Phase Attacks")]
        [SerializeField] private MonoBehaviour[] phase1AttackComponents;
        [SerializeField] private MonoBehaviour[] phase2AttackComponents;

        [Header("Facing Hitboxes")]
        [SerializeField] private Collider horizontalHitbox;
        [SerializeField] private Collider verticalHitbox;

        [Header("Presentation")]
        [SerializeField] private BossPhaseTransitionFX transitionFX;

        private HashSet<IAttackBehaviour> phase1Attacks;
        private HashSet<IAttackBehaviour> phase2Attacks;
        private bool inPhase2;

        public bool InPhase2 => inPhase2;

        protected override void Awake()
        {
            base.Awake();
            phase1Attacks = phase1AttackComponents.OfType<IAttackBehaviour>().ToHashSet();
            phase2Attacks = phase2AttackComponents.OfType<IAttackBehaviour>().ToHashSet();

            if (AnimationController != null)
                AnimationController.OnDirectionChanged += UpdateHitboxFacing;
        }

        private void OnDestroy()
        {
            if (AnimationController != null)
                AnimationController.OnDirectionChanged -= UpdateHitboxFacing;
        }

        public void UpdateHitboxFacing(int directionIndex)
        {
            bool isHorizontalFacing = directionIndex is 2 or 6;
            horizontalHitbox.enabled = isHorizontalFacing;
            verticalHitbox.enabled = !isHorizontalFacing;
        }

        protected override void Update()
        {
            if (!inPhase2 && Health.CurrentHealth / Health.MaxHealth <= phase2HealthPercent)
                EnterPhase2();

            base.Update();
        }

        [Header("Phase Transition")]
        [SerializeField] private float transitionDuration = 2f;

        private bool isTransitioning;
        public bool IsTransitioning => isTransitioning;

        private void EnterPhase2()
        {
            inPhase2 = true;
            Movement.speed *= phase2SpeedMultiplier;
            AnimationController.PlayGrowWings();
            AnimationController.SetFlyingLocomotion(true);
            transitionFX?.PlayTransition();
            StartCoroutine(TransitionRoutine());
            AudioManager.PlayOneShot(AudioDataHandler.Boss.BossFlight);
        }

        private IEnumerator TransitionRoutine()
        {
            isTransitioning = true;
            yield return new WaitForSeconds(transitionDuration);
            isTransitioning = false;
        }

        // Block all attacks while transforming
        protected override bool IsAttackUsableThisPhase(IAttackBehaviour attack)
        {
            if (isTransitioning) return false;
            return inPhase2 ? phase2Attacks.Contains(attack) : phase1Attacks.Contains(attack);
        }
    }
}
