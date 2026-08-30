using UnityEngine;

public enum UpgradeAbility
{
    DashDistance,
    SwingDamage,
    SwingSpeed,
    MoveSpeed
}

[CreateAssetMenu(fileName = "UpgradeTrack", menuName = "Upgrades/UpgradeTrack")]
public class UpgradeTrack : ScriptableObject
{
    public UpgradeAbility Ability;
    public Sprite Icon;
    public int[] CostPerRank;
    public float[] BonusPerRank;
}
