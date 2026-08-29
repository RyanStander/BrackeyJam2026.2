using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class ScrapPickup : MonoBehaviour
{
    [SerializeField] private int scrapValue = 1;
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float magnetSpeed = 100f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private Transform player;
    private float currentSpeed = 0f;
    private bool isPulling = false;

    private void OnValidate()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
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
            transform.position =
                Vector3.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || ScrapManager.Instance == null) 
            return;
        
        ScrapManager.Instance.AddScrap(scrapValue);
        EventManager.currentManager.AddEvent(new ScrapPickedUp(scrapValue));
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
