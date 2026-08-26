using System;
using System.Collections;
using System.Collections.Generic;
using Combat.Stats;
using UnityEngine;

public class ScrapDrop : MonoBehaviour
{
    public Health health;
    public GameObject scrapPrefab;

    void Awake()
    {
        health = GetComponent<Health>();
    }
    void OnEnable()
    {
        health.OnDeath += DropScrap;
    }
    void OnDisable()
    {
        health.OnDeath -= DropScrap;
    }

    private void DropScrap()
    {
        Instantiate(scrapPrefab, transform.position, Quaternion.identity);
    }
}
