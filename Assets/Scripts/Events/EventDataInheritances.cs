using Expeditions.Room;
using UnityEngine;

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

    public class OnEnemyDeath : EventData { 
        public OnEnemyDeath() : base(EventType.OnEnemyDeath){}
    }
    
    public class CommandAreaExit : EventData
    {
        public CommandAreaExit() : base(EventType.CommandAreaExit){}
    }

    public class ExecuteCombatRoom : EventData
    {
        public readonly RoomManager SourceRoom;
        public readonly CombatRoomData RoomData;
        public readonly Bounds RoomBounds;

        public ExecuteCombatRoom(RoomManager sourceRoom, RoomData roomData, Bounds roomBounds) : base(EventType.ExecuteCombatRoom)
        {
            SourceRoom = sourceRoom;
            RoomData = (CombatRoomData) roomData;
            RoomBounds = roomBounds;
        }
    }

    public class CommandRoomChange : EventData
    {
        public readonly RoomManager SourceRoom;
        public readonly RoomManager TargetRoom;
        public readonly Vector3 TargetPosition;

        public CommandRoomChange(RoomManager sourceRoom, RoomManager targetRoom, Vector3 targetPosition) : base(EventType.CommandRoomChange)
        {
            SourceRoom = sourceRoom;
            TargetRoom = targetRoom;
            TargetPosition = targetPosition;
        }
    }
}
