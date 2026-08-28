using UnityEngine;

namespace Expeditions.Room
{
    [CreateAssetMenu(fileName = "EmptyRoomData", menuName = "Expeditions/Rooms/EmptyData")]
    public class EmptyRoomData : RoomData
    {
        public override RoomType Type => RoomType.empty;
    }
}