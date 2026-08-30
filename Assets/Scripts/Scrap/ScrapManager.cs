using System.Collections;
using System.Collections.Generic;
using System;
using Events;
using UnityEngine;
using TMPro;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;
        EventManager.currentManager.AddEvent(new UpdatePlayerScrapCount(GameState.ScrapTotal));
    }

    public void AddScrap(int amount)
    {
        GameState.ScrapTotal += amount;
        EventManager.currentManager.AddEvent(new UpdatePlayerScrapCount(GameState.ScrapTotal));
    }

    public void RemoveScrap(int amount)
    {
        GameState.ScrapTotal -= amount;
        EventManager.currentManager.AddEvent(new UpdatePlayerScrapCount(GameState.ScrapTotal));
    }

    public bool TrySpendScrap(int amount)
    {
        if (GameState.ScrapTotal >= amount)
        {
            RemoveScrap(amount);
            return true;
        }

        return false;
    }

    public bool
        HasEnoughScrap(
            int amount) //maybe use to check if we have enough to purchase things on screenand grey out what we dont have enough for 
    {
        return GameState.ScrapTotal >= amount;
    }
}
