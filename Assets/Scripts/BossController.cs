using UnityEngine;

namespace Controllers
{
    public class BossController : AIController
    {
        [SerializeField] private float phase2Threshold = 15f;
        [SerializeField] private float phase2SpeedMultiplier = 1.5f;

        private bool inPhase2;

        protected override void Update()
        {
            if (!inPhase2 && Health.CurrentHealth <= phase2Threshold)
                EnterPhase2();

            base.Update();
        }

        private void EnterPhase2()
        {
            inPhase2 = true;
            Movement.speed *= phase2SpeedMultiplier;
            Animator.SetTrigger("GrowWings");
        }
    }
}