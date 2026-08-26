using UnityEngine;

namespace Combat.Data
{
    public struct DamageInfo
    {
        public float Amount;
        public Faction SourceFaction;
        public GameObject Instigator;
        public DamageMode Mode;

        public DamageInfo(float amount, Faction sourceFaction, GameObject instigator, DamageMode mode = DamageMode.Normal) {
            Amount = amount;
            SourceFaction = sourceFaction;
            Instigator = instigator;
            Mode = mode;
        }
    }
}
