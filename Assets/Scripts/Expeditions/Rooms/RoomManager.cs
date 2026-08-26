using AYellowpaper.SerializedCollections;
using Expeditions.Room.Doorway;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expeditions.Room
{
    [Serializable]
    public partial class RoomManager : MonoBehaviour
    {
        [SerializeField]
        private RoomData data;

        [field: SerializeField]
        public RoomState CurrentState { get; private set; }

        [SerializedDictionary("Direction", "Doorway")]
        public SerializedDictionary<RoomDirection, RoomDoorway> DirectionDoorwayPairs = new();

        [SerializedDictionary("Direction", "Target Room")]
        public SerializedDictionary<RoomDirection, RoomManager> DirectionRoomPairs = new();

        public List<KeyValuePair<RoomDirection, RoomDoorway>> ActiveDirectionDoorwayPairs { get; private set; } = new(); // Used to ignore inactive doors during iteration

        #region initialization
        private void InitializeDoors()
        {
            ActiveDirectionDoorwayPairs.Clear();
            foreach (var pair in DirectionDoorwayPairs)
            {
                DirectionRoomPairs.TryGetValue(pair.Key, out RoomManager manager);
                bool is_active = manager && DirectionRoomPairs.ContainsKey(pair.Key);
                pair.Value.gameObject.SetActive(is_active);

                if (is_active) {
                    ActiveDirectionDoorwayPairs.Add(pair);
                    pair.Value.Initialize(this, manager, pair.Key);
                }
            }
        }
        private void OnEnable()
        {
            InitializeDoors();
            UpdateRoom();
        }
        #endregion

        public void UpdateRoom(RoomState targetState = RoomState.undefined)
        {
            if (targetState != RoomState.undefined) { CurrentState = targetState; }
            foreach (var pair in ActiveDirectionDoorwayPairs)
            {
                pair.Value.UpdateState(CurrentState);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            foreach (var pair in DirectionRoomPairs)
            {
                if (pair.Value)
                {
                    Gizmos.DrawLine(this.gameObject.transform.position, pair.Value.gameObject.transform.position);
                }
            }
        }
        private void OnValidate()
        {
            //InitializeDoors();
            //EditorUtility.SetDirty(this);
        }
#endif
    }
}

