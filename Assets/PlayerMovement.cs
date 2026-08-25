using System.Collections;
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

    public Vector3 direction;

    public Rigidbody playerRB;
    private Vector2 playerMovementInput;
    public float lungeCooldown = 1f;
    private float nextLungeTime;
    private Health health;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        playerMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (playerMovementInput.sqrMagnitude > 0f)
        {
            float angle = Mathf.Atan2(playerMovementInput.x, playerMovementInput.y) * Mathf.Rad2Deg;

            angle = Mathf.Round(angle / 45f) * 45f;

            //transform.rotation = Quaternion.Euler(0f, 0f, -angle);
            //transform.rotation = Quaternion.Euler(0f, -angle, 0f);
            transform.rotation = Quaternion.Euler(90f, angle, 0f); //got it so we actually rotate, no more bs direction code outside this, can use trans.forward
            
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //moveSpeed *= 2f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //moveSpeed /= 2f;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Lunge();
        }
    }

    private void Lunge()
    {
        health.iFrameActive = true;
        if(Time.time >= nextLungeTime)
        {
            StartCoroutine(LungeRoutine());
            nextLungeTime = Time.time + lungeCooldown;
        }
        health.iFrameActive = false;
    }

    private IEnumerator LungeRoutine()
    {
        float duration = 0.15f;
        float distance = 1.5f;
        Vector3 dir = transform.up;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            playerRB.MovePosition(playerRB.position + dir * (distance / duration) * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        direction = new Vector3(playerMovementInput.x, 0f, playerMovementInput.y).normalized;
        playerRB.MovePosition(playerRB.position + direction * moveSpeed * Time.fixedDeltaTime);

    }
}