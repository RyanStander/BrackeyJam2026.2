using System.Collections;
using System.Collections.Generic;
using System;
using Events;
using UnityEngine;
using TMPro;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager Instance { get; private set; }
    [SerializeField] private int startingScrap = 0;
    [SerializeField] private int currentScrap;
    public event Action<int> OnScrapChanged;

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
        currentScrap = startingScrap;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;
        currentScrap = startingScrap;
        EventManager.currentManager.AddEvent(new UpdatePlayerScrapCount(currentScrap));
    }

    public void AddScrap(int amount)
    {
        currentScrap += amount;
        OnScrapChanged?.Invoke(currentScrap);
        EventManager.currentManager.AddEvent(new UpdatePlayerScrapCount(currentScrap));
    }
    public void RemoveScrap(int amount)
    {
        currentScrap -= amount;
        OnScrapChanged?.Invoke(currentScrap);
        EventManager.currentManager.AddEvent(new UpdatePlayerScrapCount(currentScrap));
    }
    public bool TrySpendScrap(int amount)
    {
        if (currentScrap >= amount)
        {
            RemoveScrap(amount);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool HasEnoughScrap(int amount) //maybe use to check if we have enough to purchase things on screenand grey out what we dont have enough for 
    {
        return currentScrap >= amount;
    }
    public void ResetScrap()
    {
        currentScrap = startingScrap;
        OnScrapChanged?.Invoke(currentScrap);
        EventManager.currentManager.AddEvent(new UpdatePlayerScrapCount(currentScrap));
    }
}
