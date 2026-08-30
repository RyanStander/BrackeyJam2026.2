using System.Collections;
using Combat.Animations;
using Player;
using UnityEngine;

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

    #region State

    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private PlayerAttack playerAttack;

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
        if (animationController == null)
            animationController = GetComponent<PlayerAnimationController>();
        if (playerAttack == null)
            playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        ReadMovementInput();
        int directionIndex = GetDirectionIndex(movementInput);
        animationController.UpdateDirection(directionIndex,movementInput.sqrMagnitude);
        playerAttack.SetAttackDirection(directionIndex,movementInput.sqrMagnitude);
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
