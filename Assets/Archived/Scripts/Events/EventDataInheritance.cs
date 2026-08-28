using Expeditions.Room;
using UnityEngine;

namespace Events {
    public class ExecuteCombatRoom : EventData
    {
        public readonly RoomManager SourceRoom;
        public readonly CombatRoomData RoomData;
        public readonly Bounds RoomBounds;

        public ExecuteCombatRoom(RoomManager sourceRoom, RoomData roomData, Bounds roomBounds) : base(EventType.ExecuteCombatRoom)
        {
            SourceRoom = sourceRoom;
            RoomData = (CombatRoomData)roomData;
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