using System;
using System.Collections;
using Player;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat.Animations
{
    public class DeathFallEffect : MonoBehaviour
    {
        [SerializeField] private Rigidbody characterRigidbody;
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerAttack playerAttack;

        [SerializeField] private float upwardPushForce = 3f;
        [SerializeField] private float tumbleTorque = 5f;
        [SerializeField] private float settleDelay = 0.6f;
        [SerializeField] private float shrinkFadeDuration = 0.5f;

        private void OnValidate()
        {
            if (characterRigidbody==null)
                characterRigidbody= GetComponent<Rigidbody>();
            if (skeletonAnimation==null)
                skeletonAnimation= GetComponentInChildren<SkeletonAnimation>();
            if (animationController==null)
                animationController= GetComponent<PlayerAnimationController>();
            if (playerMovement==null)
                playerMovement= GetComponent<PlayerMovement>();
            if (playerAttack==null)
               playerAttack = GetComponent<PlayerAttack>();
        }

        public void PlayDeath()
        {
            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            animationController.FreezeOnDeath();
            playerMovement.enabled = false;
            playerAttack.enabled = false;
            
            characterRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            characterRigidbody.useGravity = true;
            characterRigidbody.AddForce(Vector3.up * upwardPushForce, ForceMode.Impulse);
            characterRigidbody.AddTorque(transform.forward * tumbleTorque, ForceMode.Impulse);

            yield return new WaitForSeconds(settleDelay);
            yield return StartCoroutine(ShrinkFadeRoutine());
        }

        private IEnumerator ShrinkFadeRoutine()
        {
            Vector3 startScale = transform.localScale;
            Color skeletonColour = skeletonAnimation.skeleton.GetColor();
            float elapsed = 0f;

            while (elapsed < shrinkFadeDuration)
            {
                float t = elapsed / shrinkFadeDuration;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                skeletonColour.a = Mathf.Lerp(skeletonColour.a, 0f, t);
                skeletonAnimation.skeleton.SetColor(skeletonColour);
                elapsed += Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
