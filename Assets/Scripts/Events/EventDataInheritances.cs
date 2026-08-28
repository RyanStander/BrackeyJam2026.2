
namespace Events
{
    //Event that informs subscribers of a debug log
    public class SendDebugLog : EventData
    {
        public readonly string Debuglog;

        public SendDebugLog(string givenLog) : base(EventType.ReceiveDebug)
        {
            Debuglog = givenLog;
        }
    }
    
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

    public class ReturnToHub : EventData
    {
        public ReturnToHub() : base(EventType.ReturnToHub) { }
    }

    public class OnEnemyDeath : EventData { 
        public OnEnemyDeath() : base(EventType.OnEnemyDeath){}
    }

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
}
