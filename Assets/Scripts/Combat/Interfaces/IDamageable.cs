using Combat.Data;
using UnityEngine;

namespace Combat.Interfaces
{
    public interface IDamageable
    {  
        Faction Faction { get; }
        GameObject GameObject { get; }
        void TakeDamage(float damage);
    }
}
