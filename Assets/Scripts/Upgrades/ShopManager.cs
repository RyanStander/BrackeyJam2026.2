using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UpgradeSlot
{
    public UpgradeTrack Track;
    public Button UpgradeButton;
    public TMP_Text CostText;
}

public class ShopManager : MonoBehaviour
{
    [SerializeField] private UpgradeSlot[] slots;

    private void Start()
    {
        foreach (UpgradeSlot slot in slots)
        {
            UpgradeSlot capturedSlot = slot;
            slot.UpgradeButton.onClick.AddListener(() => TryUpgrade(capturedSlot));
            RefreshSlot(capturedSlot);
        }
    }

    private void TryUpgrade(UpgradeSlot slot)
    {
        int rank = GetRank(slot.Track.Ability);

        if (rank >= slot.Track.CostPerRank.Length)
            return;

        int cost = slot.Track.CostPerRank[rank];

        if (!ScrapManager.Instance.TrySpendScrap(cost))
            return;

        SetRank(slot.Track.Ability, rank + 1);
        RefreshSlot(slot);
    }

    private void RefreshSlot(UpgradeSlot slot)
    {
        int rank = GetRank(slot.Track.Ability);

        if (rank >= slot.Track.CostPerRank.Length)
        {
            slot.CostText.text = "MAX";
            slot.UpgradeButton.interactable = false;
        }
        else
        {
            slot.CostText.text = "Cost: " + slot.Track.CostPerRank[rank] + " Scrap";
            slot.UpgradeButton.interactable = true;
        }
    }

    private int GetRank(UpgradeAbility ability)
    {
        switch (ability)
        {
            case UpgradeAbility.DashDistance: return GameState.DashRank;
            case UpgradeAbility.SwingDamage: return GameState.DamageRank;
            case UpgradeAbility.SwingSpeed: return GameState.SwingSpeedRank;
            case UpgradeAbility.MoveSpeed: return GameState.MoveSpeedRank;
            default: return 0;
        }
    }

    private void SetRank(UpgradeAbility ability, int rank)
    {
        switch (ability)
        {
            case UpgradeAbility.DashDistance: GameState.DashRank = rank; break;
            case UpgradeAbility.SwingDamage: GameState.DamageRank = rank; break;
            case UpgradeAbility.SwingSpeed: GameState.SwingSpeedRank = rank; break;
            case UpgradeAbility.MoveSpeed: GameState.MoveSpeedRank = rank; break;
        }
    }
}
