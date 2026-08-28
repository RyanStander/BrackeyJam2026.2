
namespace Events
{
    
    #region Factory
    
    public class CreateEnemy : EventData
    {
        public readonly Factories.EnemyType EnemyType;
        public readonly UnityEngine.Vector3 Position;

        public CreateEnemy(Factories.EnemyType enemyType, UnityEngine.Vector3 position) : base(EventType.CreateEnemy)
        {
            EnemyType = enemyType;
            Position = position;
        }
    }
    
    public class CreatePickup : EventData
    {
        public readonly Factories.PickupType PickupType;
        public readonly UnityEngine.Vector3 Position;

        public CreatePickup(Factories.PickupType pickupType, UnityEngine.Vector3 position) : base(EventType.CreatePickup)
        {
            PickupType = pickupType;
            Position = position;
        }
    }
    
    #endregion

    #region Wave System

    public class WaveStart : EventData
    {
        public WaveStart() : base(EventType.WaveStart) { }
    }
    public class WaveEnd : EventData
    {
        public WaveEnd() : base(EventType.WaveEnd) { }
    }

    public class BossStart : EventData
    {
        public BossStart() : base(EventType.BossStart) { }
    }
    public class BossEnd : EventData
    {
        public BossEnd() : base(EventType.BossEnd) { }
    }

    public class WavesCompleted : EventData
    {
        public WavesCompleted() : base(EventType.WavesCompleted) { }
    }

    #endregion
    
    public class ReturnToHub : EventData
    {
        public ReturnToHub() : base(EventType.ReturnToHub) { }
    }

    public class OnEnemyDeath : EventData { 
        public OnEnemyDeath() : base(EventType.OnEnemyDeath){}
    }

    #region UI Displays

    public class UpdatePlayerScrapCount: EventData
    {
        public readonly int CurrentScrap;

        public UpdatePlayerScrapCount(int currentScrap) : base(EventType.UpdatePlayerScrapCount)
        {
            CurrentScrap = currentScrap;
        }
    }
    
    public class UpdatePlayerHealth: EventData
    {
        public readonly float CurrentHealth;

        public UpdatePlayerHealth(float currentHealth) : base(EventType.UpdatePlayerHealth)
        {
            CurrentHealth = currentHealth;
        }
    }
    
    public class SetPlayerHealth: EventData
    {
        public readonly float CurrentHealth;
        public readonly float MaxHealth;

        public SetPlayerHealth(float currentHealth, float maxHealth) : base(EventType.SetPlayerHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }

    #endregion
    
    
}
