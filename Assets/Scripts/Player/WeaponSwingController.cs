using UnityEngine;

namespace Player
{
    public class WeaponSwingController : MonoBehaviour
    {
        #region Config

        [SerializeField] private Transform weaponPivot;
        [SerializeField] private Animator weaponAnimator;
        [SerializeField] private TrailRenderer weaponTrail;
        [SerializeField] private float swingDuration = 0.2f;
        [SerializeField] private GameObject weaponVisual;

        // Maps direction index (0-7) to a Y rotation. Calibrate these against
        // your actual pivot's rest orientation - signs/offset may need flipping
        // once tested in-scene.
        [SerializeField] private float[] directionYRotations = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };

        #endregion

        #region State

        private static readonly int swingTrigger = Animator.StringToHash("Swing");
        private float trailEndTime;

        #endregion

        #region Public API

        public void PlaySwing(int directionIndex, Vector3 swingOffset)
        {
            if (directionIndex < 0 || directionIndex >= directionYRotations.Length) return;

            weaponPivot.localPosition = swingOffset;
            weaponPivot.localRotation = Quaternion.Euler(0f, directionYRotations[directionIndex], 0f);
            weaponAnimator.SetTrigger(swingTrigger);

            StartTrail();
        }

        #endregion

        #region Trail

        private void StartTrail()
        {
            weaponTrail.Clear();
            weaponTrail.emitting = true;
            weaponVisual.SetActive(true);
            trailEndTime = Time.time + swingDuration;
        }

        private void Update()
        {
            if (weaponTrail.emitting && Time.time >= trailEndTime)
            {
                weaponTrail.emitting = false;
                weaponVisual.SetActive(false);
            }
        }

        #endregion
    }
}
