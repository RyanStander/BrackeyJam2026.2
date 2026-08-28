namespace Expeditions.Room
{
    public enum RoomDirection
    {
        undefined, up, down, left, right
    }

    public static class RoomDirectionExtensions
    {
        public static RoomDirection GetCounterpart(RoomDirection direction)
        {
            switch (direction)
            {
                case RoomDirection.up: return RoomDirection.down;
                case RoomDirection.down: return RoomDirection.up;
                case RoomDirection.left: return RoomDirection.right;
                case RoomDirection.right: return RoomDirection.left;
                default: return RoomDirection.undefined;
            }
        }
    }
}

