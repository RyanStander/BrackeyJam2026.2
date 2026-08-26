using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class ScrapManager : MonoBehaviour
{
    public static ScrapManager Instance { get; private set; }
    [SerializeField] private int startingScrap = 0;
    [SerializeField] public int CurrentScrap;
    public event Action<int> OnScrapChanged;

    public TextMeshProUGUI scrapText; 
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        CurrentScrap = startingScrap;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CurrentScrap = startingScrap;
        scrapText.text = "Current Scrap: " + CurrentScrap.ToString();
    }

    public void AddScrap(int amount)
    {
        CurrentScrap += amount;
        OnScrapChanged?.Invoke(CurrentScrap);
        scrapText.text = "Current Scrap: " + CurrentScrap.ToString();
    }
    public void RemoveScrap(int amount)
    {
        CurrentScrap -= amount;
        OnScrapChanged?.Invoke(CurrentScrap);
        scrapText.text = "Current Scrap: " + CurrentScrap.ToString();
    }
    public bool TrySpendScrap(int amount)
    {
        if (CurrentScrap >= amount)
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
        return CurrentScrap >= amount;
    }
    public void ResetScrap()
    {
        CurrentScrap = startingScrap;
        OnScrapChanged?.Invoke(CurrentScrap);
        scrapText.text = "Current Scrap: " + CurrentScrap.ToString();
    }
}
