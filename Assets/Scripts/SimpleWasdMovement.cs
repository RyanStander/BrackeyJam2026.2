using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWasdMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}
