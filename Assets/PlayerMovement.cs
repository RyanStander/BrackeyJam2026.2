using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //summary
    // This script handles the movement of the player character in a 3D plane but with a static camera with the intention of Spine2D spirtes 
    // It allows the player to move in 8 directions). The player's visual representation rotates to face the direction of movement.
    // angle is calculated  using the input then rounded to the nearest 45 degrees to ensure the player faces one of the 8 cardinal directions
    // using rigibody to allow for better movement and collision detection
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform playerVisual;

    private Rigidbody playerRB;
    private Vector2 playerMovementInput;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (playerMovementInput.sqrMagnitude > 0f)
        {
            float angle = Mathf.Atan2(playerMovementInput.x, playerMovementInput.y) * Mathf.Rad2Deg;

            angle = Mathf.Round(angle / 45f) * 45f;

            playerVisual.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 2f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 2f;
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(playerMovementInput.x, 0f, playerMovementInput.y).normalized;
        playerRB.MovePosition(playerRB.position + direction * moveSpeed * Time.fixedDeltaTime);

    }
}