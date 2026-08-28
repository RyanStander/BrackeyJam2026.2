using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Config

        [SerializeField] private SkeletonAnimation skeletonAnimation;

        [Header("Diagonal Fake")]
        [Tooltip("Fake rotation for making extra 4 diagonal angles.")]
        [SerializeField] private float diagonalTiltAngle = 18f;

        private const string idleBackAnimationName = "Idle/MC_Back_Idle";
        private const string idleFrontAnimationName = "Idle/MC_Front_Idle";
        private const string idleSideAnimationName = "Idle/MC_Side_Idle";
        private const string frontAttackAnimationName = "MC_Front_Attack";
        private const string backRunAnimationName = "Move/MC_Back_Run";
        private const string frontRunAnimationName = "Move/MC_Front_Run";
        private const string sideRunAnimationName = "Move/MC_Side_Run";

        #endregion

        #region State

        private AnimationState animationState;
        private string currentRunAnimationName = "";
        private string currentIdleAnimationName = "";

        #endregion

        #region Unity Lifecycle

        private void OnValidate()
        {
            if (skeletonAnimation == null)
                skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        }

        private void Awake()
        {
            animationState = skeletonAnimation.AnimationState;
        }

        #endregion

        #region Public API

        public void UpdateDirection(int directionIndex, float movementMagnitude)
        {
            if (movementMagnitude <= 0f)
            {
                SetIdleDirectionAnimation();
            }
            else
            {
                //0=forward
                //1=up diagonal left
                //2=left
                //3=down diagonal left
                //4=down
                //5=down diagonal right
                //6=right
                //7=up diagonal right
                SetRunDirectionAnimation(directionIndex);
            }
        }

        #endregion

        #region Animation Selection

        private void SetRunDirectionAnimation(int directionIndex)
        {
            string targetAnimation = GetRunDirectionAnimation(directionIndex);
            if (targetAnimation == "") return;

            if (targetAnimation != currentRunAnimationName)
            {
                animationState.SetAnimation(0, targetAnimation, true);
                currentRunAnimationName = targetAnimation;
                currentIdleAnimationName = "";
            }
            
            UpdateFacingScale(directionIndex);
        }

        private void SetIdleDirectionAnimation()
        {
            string targetAnimation = GetIdleDirectionAnimation();
            if (targetAnimation == "") return;

            if (targetAnimation != currentIdleAnimationName)
            {
                animationState.SetAnimation(0, targetAnimation, true);
                currentIdleAnimationName = targetAnimation;
                currentRunAnimationName = "";
            }

            // Reset tilt when idle - only the run/moving state fakes diagonals.
            ApplyDiagonalTilt(-1);
        }

        private void UpdateFacingScale(int directionIndex)
        {
            switch (directionIndex)
            {
                case >= 1 and <= 5:
                    skeletonAnimation.skeleton.ScaleX = -1;
                    break;
                case 6 or 7 or 0:
                    skeletonAnimation.skeleton.ScaleX = 1;
                    break;
            }

            ApplyDiagonalTilt(directionIndex);
        }

        /// <summary>
        /// Fakes diagonal facing by rotating the skeleton slightly on top of the nearest
        /// cardinal (front/back/side) run animation, since only 4 base directions are authored.
        /// Rotation sign is chosen per-direction so diagonals tilt toward the correct side
        /// regardless of the ScaleX mirror applied alongside it.
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
                default:
                    skeletonAnimation.transform.localRotation = Quaternion.identity;
                    break;
            }
        }

        private string GetRunDirectionAnimation(int directionIndex)
        {
            switch (directionIndex)
            {
                case 0: return backRunAnimationName;
                case 1: return backRunAnimationName;
                case 2: return sideRunAnimationName;
                case 3: return frontRunAnimationName;
                case 4: return frontRunAnimationName;
                case 5: return frontRunAnimationName;
                case 6: return sideRunAnimationName;
                case 7: return backRunAnimationName;
            }
            return "";
        }

        private string GetIdleDirectionAnimation()
        {
            switch (currentRunAnimationName)
            {
                case backRunAnimationName: return idleBackAnimationName;
                case frontRunAnimationName: return idleFrontAnimationName;
                case sideRunAnimationName: return idleSideAnimationName;
            }
            return "";
        }

        #endregion
    }
}
