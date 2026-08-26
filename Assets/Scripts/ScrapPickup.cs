using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapPickup : MonoBehaviour
{
    private int scrapValue = 1;
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float magnetSpeed = 100f;
    [SerializeField] private float acceleration = 50f;
    private Transform player;
    private float currentSpeed = 0f;
    private bool isPulling = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; //horrible i know..
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= magnetRadius && !isPulling)
        {
            isPulling = true;
        }
        else
        {
            isPulling = false;
        }
        if (isPulling)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, magnetSpeed, acceleration * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScrapManager.Instance != null)
            {
                ScrapManager.Instance.AddScrap(scrapValue);
                Destroy(gameObject);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
