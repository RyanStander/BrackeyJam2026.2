using Events;
using UnityEngine;

namespace Expeditions.Room.Doorway
{
    public partial class RoomDoorway : MonoBehaviour
    {
        #region Setup
        [SerializeField]
        private BoxCollider exitCollider;

        [SerializeField]
        private BoxCollider blockCollider;

        [SerializeField]
        private GameObject blockVisual;

        [field:SerializeField]
        public GameObject TransferPoint { get; private set; }
        #endregion

        private RoomState currentState;

        private RoomManager SourceRoom, TargetRoom;
        private RoomDirection TransferDirection;

        public void Initialize(RoomManager source, RoomManager target, RoomDirection direction)
        {
            SourceRoom = source;
            TargetRoom = target;
            TransferDirection = direction;
        }

        public void UpdateState(RoomState state)
        {
            currentState = state;
            bool is_locked = state is RoomState.locked;
            blockVisual.SetActive(is_locked);

            blockCollider.enabled = is_locked;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (TargetRoom.DirectionDoorwayPairs.TryGetValue(RoomDirectionExtensions.GetCounterpart(TransferDirection), out RoomDoorway doorway))
                {
                    EventManager.currentManager.AddEvent(new CommandRoomChange(SourceRoom, TargetRoom, doorway.TransferPoint.transform.position));
                }
            }
        }

#if UNITY_EDITOR
        private Color GetColor(RoomState state)
        {
            switch (state)
            {
                case RoomState.unknown: return Color.gray;
                case RoomState.unexplored: return Color.yellow;
                case RoomState.locked: return Color.red;
                case RoomState.explored: return Color.green;
                default: return Color.white;
            }
        }
        private void OnDrawGizmos()
        {
            if (!exitCollider || !blockCollider) return;
            Gizmos.color = GetColor(currentState);
            Gizmos.DrawWireCube(exitCollider.bounds.center, exitCollider.bounds.size);
            Gizmos.DrawWireCube(blockCollider.bounds.center, blockCollider.bounds.size);
        }
    }
#endif
}