using System;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Combat.Animations
{
    public class AIAnimationController : MonoBehaviour
    {
        [Header("Optional - pick whichever this AI actually uses")] [SerializeField]
        private Animator animator;

        [SerializeField] private SkeletonAnimation skeletonAnimation;

        [Header("Spine Direction Animation Names")] [SerializeField]
        private string runFrontAnimationName;

        [SerializeField] private string runBackAnimationName;
        [SerializeField] private string runSideAnimationName;
        [SerializeField] private string shootFrontAnimationName;
        [SerializeField] private string shootBackAnimationName;
        [SerializeField] private string shootSideAnimationName;
        [SerializeField] private string stunFrontAnimationName;
        [SerializeField] private string stunBackAnimationName;
        [SerializeField] private string stunSideAnimationName;

        [Header("Diagonal Fake")]
        [Tooltip(
            "Rotation (degrees) applied on top of the nearest front/back/side animation to fake a diagonal facing.")]
        [SerializeField]
        private float diagonalTiltAngle = 18f;

        private enum FacingCategory
        {
            Front,
            Back,
            Side
        }

        private AnimationState spineState;
        private string currentSpineAnimation = "";

        private void OnValidate()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            if (skeletonAnimation == null)
                skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void Awake()
        {
            if (skeletonAnimation != null)
                spineState = skeletonAnimation.AnimationState;
        }

        #region Public API

        public void PlayRun(Vector3 direction, float speed = 1f)
        {
            int index = GetDirectionIndex(direction);
            var (category, faceLeft) = GetFacing(index);

            if (animator != null)
            {
                animator.speed = speed;
                animator.SetTrigger("Run"); // adjust name to match your Animator setup
            }

            if (spineState != null)
            {
                string clip = category switch
                {
                    FacingCategory.Front => runFrontAnimationName,
                    FacingCategory.Back => runBackAnimationName,
                    FacingCategory.Side => runSideAnimationName,
                    _ => ""
                };
                PlaySpine(clip, true, faceLeft, index,speed);
            }
        }

        public void PlayShoot(Vector3 direction)
        {
            int index = GetDirectionIndex(direction);
            var (category, faceLeft) = GetFacing(index);

            if (animator != null)
            {
                animator.speed = 1f;
                animator.SetTrigger("Shoot");
            }

            if (spineState != null)
            {
                string clip = category switch
                {
                    FacingCategory.Front => shootFrontAnimationName,
                    FacingCategory.Back => shootBackAnimationName,
                    FacingCategory.Side => shootSideAnimationName,
                    _ => ""
                };
                PlaySpine(clip, false, faceLeft, index);
            }
        }
        
        public void PlayStun(Vector3 direction)
        {
            int index = GetDirectionIndex(direction);
            var (category, faceLeft) = GetFacing(index);

            if (animator != null)
            {
                animator.speed = 1f;
                animator.SetTrigger("Stunned");
            }

            if (spineState != null)
            {
                string clip = category switch
                {
                    FacingCategory.Front => stunFrontAnimationName,
                    FacingCategory.Back => stunBackAnimationName,
                    FacingCategory.Side => stunSideAnimationName,
                    _ => ""
                };
                PlaySpine(clip, false, faceLeft, index);
            }
        }

        /// <summary>
        /// No idle exists - freeze on whatever frame is currently showing instead
        /// of looping a stand-still animation. Used for Reload and End phases.
        /// </summary>
        public void PauseOnCurrentFrame()
        {
            if (animator != null)
                animator.speed = 0f;

            if (spineState != null)
                spineState.TimeScale = 0f;
        }

        public void Resume()
        {
            if (animator != null)
                animator.speed = 1f;

            if (spineState != null)
                spineState.TimeScale = 1f;
        }

        #endregion

        #region Internal

        private void PlaySpine(string clipName, bool loop, bool faceLeft, int directionIndex, float speed=1f)
        {
            if (string.IsNullOrEmpty(clipName)) return;

            skeletonAnimation.skeleton.ScaleX = faceLeft ? 1f : -1f;
            ApplyDiagonalTilt(directionIndex);

            if (clipName != currentSpineAnimation)
            {
                spineState.SetAnimation(0, clipName, loop);
                currentSpineAnimation = clipName;
            }

            spineState.TimeScale = speed; // in case a previous phase paused it
        }

        /// <summary>
        /// Fakes diagonal facing by rotating the skeleton slightly on top of the nearest
        /// front/back/side animation, since only cardinal directions are authored.
        /// Rotation sign is chosen per-direction so diagonals tilt toward the correct side
        /// regardless of the ScaleX mirror applied alongside it. Mirrors the same trick
        /// used in PlayerAnimationController - keep the sign convention consistent with that.
        /// </summary>
        private void ApplyDiagonalTilt(int directionIndex)
        {
            float tilt = diagonalTiltAngle;

            switch (directionIndex)
            {
                case 1: // up diagonal left
                    skeletonAnimation.transform.localRotation = Quaternion.Euler(0f, tilt, 0f);
                    break;
                case 3: // down diagonal left
                    skeletonAnimation.transform.localRotation = Quaternion.Euler(0f, -tilt, 0f);
                    break;
                case 5: // down diagonal right
                    skeletonAnimation.transform.localRotation = Quaternion.Euler(0f, tilt, 0f);
                    break;
                case 7: // up diagonal right
                    skeletonAnimation.transform.localRotation = Quaternion.Euler(0f, -tilt, 0f);
                    break;
                default: // pure cardinal directions (0, 2, 4, 6) - no tilt
                    skeletonAnimation.transform.localRotation = Quaternion.identity;
                    break;
            }
        }

        private int GetDirectionIndex(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float snapped = Mathf.Round(angle / 45f) * 45f;
            return ((Mathf.RoundToInt(snapped / 45f) % 8) + 8) % 8;
        }

        private (FacingCategory, bool faceLeft) GetFacing(int index)
        {
            FacingCategory category = index switch
            {
                0 or 1 or 7 => FacingCategory.Back,
                2 or 6 => FacingCategory.Side,
                _ => FacingCategory.Front, // 3, 4, 5
            };

            bool faceLeft = index is >= 5 or 6 or 7 or 0;

            return (category, faceLeft);
        }

        #endregion
    }
}
