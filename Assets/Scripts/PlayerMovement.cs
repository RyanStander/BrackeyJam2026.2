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
    [SerializeField] private float acceleration = 12f;
    private Vector3 currentVelocity;
    [SerializeField] private float lungeCooldown = 1f;
    [SerializeField] private float lungeDuration = 0.5f;
    [SerializeField] private float lungeDistance = 5f;
    [SerializeField] private float lungeSpeed = 10f;

    [SerializeField] private SpriteRenderer visualRenderer;
    [SerializeField] private Sprite[] directionSprites = new Sprite[8];
    
    private float nextLungeTime;
    private Transform playerVisual;
    public Vector3 direction;
    public Rigidbody playerRB;
    private Vector2 playerMovementInput;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        playerMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (playerMovementInput.sqrMagnitude > 0f)
        {
            float angle = Mathf.Atan2(playerMovementInput.x, playerMovementInput.y) * Mathf.Rad2Deg;

            angle = Mathf.Round(angle / 45f) * 45f;

            int i = ((Mathf.RoundToInt(angle / 45f)) % 8 + 8) % 8;

            if (visualRenderer && directionSprites[i]) 
            {
                visualRenderer.sprite = directionSprites[i];
                //transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            //transform.rotation = Quaternion.Euler(0f, 0f, -angle);
            //transform.rotation = Quaternion.Euler(0f, -angle, 0f);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f); //got it so we actually rotate, no more bs direction code outside this, can use trans.forward
        // this is now broken and we aren't actually turning our gameobject just the sprite..
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
        if (Time.time >= nextLungeTime)
        {
            nextLungeTime = Time.time + lungeCooldown;
            StartCoroutine(LungeRoutine());
        }
    }

    private IEnumerator LungeRoutine()
    {
        playerHealth.TriggerIFrames(lungeDuration); //going off the extended player heahth to handle
        float duration = lungeDuration;
        float distance = lungeDistance;
        Vector3 dir = new Vector3(playerMovementInput.x, 0f, playerMovementInput.y).normalized;
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
        Vector3 targetVelocity = direction * moveSpeed;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        playerRB.MovePosition(playerRB.position + currentVelocity * Time.fixedDeltaTime);
    }
}