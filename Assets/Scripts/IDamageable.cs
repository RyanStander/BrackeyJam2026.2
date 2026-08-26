using UnityEngine;

namespace Combat.Interfaces
{
    public interface IDamageable
    {  
        GameObject GameObject { get; }
        void TakeDamage(float damage);
    }
}