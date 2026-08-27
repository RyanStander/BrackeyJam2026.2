using System;

namespace Expeditions.Room
{
    [Serializable]
    public enum RoomState
    {
        undefined, undiscovered, unexplored, locked, explored
    }
}