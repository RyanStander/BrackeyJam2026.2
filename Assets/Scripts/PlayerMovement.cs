using System;
using System.Collections;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

/// <summary>
/// Handles player movement in a 3D plane under a static camera, driving 2D (Spine) sprite
/// facing for 8-directional movement. Movement uses a Rigidbody for collision detection;
/// facing direction is derived from input and snapped to the nearest 45-degree increment
/// so the player always faces one of 8 cardinal directions.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Config - Movement

    [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 12f;

    #endregion

    #region Config - Lunge

    [Header("Lunge")] [SerializeField] private float lungeCooldown = 1f;
    [SerializeField] private float lungeDuration = 0.5f;
    [SerializeField] private float lungeDistance = 5f;

    #endregion

    #region Config - Visuals

    [Header("Visuals")] [SerializeField] private SkeletonAnimation skeletonAnimation;
    private AnimationState animationState;
    private string currentRunAnimationName;
    private string currentIdleAnimationName;
    private const string idleBackAnimationName = "Idle/MC_Back_Idle";
    private const string idleFrontAnimationName = "Idle/MC_Front_Idle";
    private const string idleSideAnimationName = "Idle/MC_Side_Idle";
    private string frontAttackAnimationName = "MC_Front_Attack";
    private const string backRunAnimationName = "Move/MC_Back_Run";
    private const string frontRunAnimationName = "Move/MC_Front_Run";
    private const string sideRunAnimationName = "Move/MC_Side_Run";

    #endregion

    #region State

    private Rigidbody playerRb;
    private PlayerHealth playerHealth;

    private Vector2 movementInput;
    private Vector3 currentVelocity;
    private float nextLungeTime;

    #endregion

    #region Unity Lifecycle

    private void OnValidate()
    {
        if (playerRb == null)
            playerRb = GetComponent<Rigidbody>();
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();
        if (skeletonAnimation == null)
            skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    private void Awake()
    {
        animationState = skeletonAnimation.AnimationState;
    }

    private void Update()
    {
        ReadMovementInput();
        UpdateFacingSprite();
        HandleLungeInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    #endregion

    #region Movement

    private void ReadMovementInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        Vector3 targetVelocity = moveDirection * moveSpeed;

        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        playerRb.MovePosition(playerRb.position + currentVelocity * Time.fixedDeltaTime);
    }

    #endregion

    #region Facing Sprite

    private void UpdateFacingSprite()
    {
        if (movementInput.sqrMagnitude <= 0f)
        {
            SetIdleDirectionAnimation();
        }
        else
            //0=forward
            //1=up diagonal left
            //2=left
            //3=down diagonal left
            //4=down
            //5=down diagonal right
            //6=right
            //7=up diagonal right
            SetRunDirectionAnimation(GetDirectionIndex(movementInput));
    }

    private void SetRunDirectionAnimation(int directionIndex)
    {
        string targetAnimation = GetRunDirectionAnimation(directionIndex);

        if (targetAnimation == "")
            return;

        if (targetAnimation != currentRunAnimationName)
        {
            animationState.SetAnimation(0, targetAnimation, true);
            currentRunAnimationName = targetAnimation;
            currentIdleAnimationName = "";
            UpdateFacingScale(directionIndex);
        }
    }

    private void SetIdleDirectionAnimation()
    {
        string targetAnimation = GetIdleDirectionAnimation();

        if (targetAnimation == "")
            return;
        
        if (targetAnimation != currentIdleAnimationName)
        {
            animationState.SetAnimation(0, targetAnimation, true);
            currentIdleAnimationName = targetAnimation;
            currentRunAnimationName = "";
        }
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

    /// <summary>
    /// Converts an input vector into one of 8 cardinal direction indices (0-7),
    /// snapped to the nearest 45-degree increment.
    /// </summary>
    private int GetDirectionIndex(Vector2 input)
    {
        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(angle / 45f) * 45f;
        int index = Mathf.RoundToInt(snappedAngle / 45f);

        return ((index % 8) + 8) % 8; // wrap into 0-7 regardless of sign
    }

    #endregion

    #region Lunge

    private void HandleLungeInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            TryLunge();
    }

    private void TryLunge()
    {
        if (Time.time < nextLungeTime) return;

        nextLungeTime = Time.time + lungeCooldown;
        StartCoroutine(LungeRoutine());
    }

    private IEnumerator LungeRoutine()
    {
        playerHealth.TriggerIFrames(lungeDuration);

        Vector3 lungeDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        float speed = lungeDistance / lungeDuration;
        float elapsed = 0f;

        while (elapsed < lungeDuration)
        {
            playerRb.MovePosition(playerRb.position + lungeDirection * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    #endregion
}
