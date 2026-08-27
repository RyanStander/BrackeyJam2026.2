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
            if (CurrentState is RoomState.undefined) CurrentState = RoomState.undiscovered;
            InitializeDoors();
            UpdateRoom();
        }
        #endregion

        private void InitiateCombat()
        {
            // TODO: Replace this setup with messaging to the helper managers instead, this is just for prototyping/testing.
            CombatRoomHandler CombatHandler = FindObjectOfType<CombatRoomHandler>();
            BoxCollider GroundMesh = GetComponent<BoxCollider>();
            CombatHandler.SpawnEnemiesTest((CombatRoomData) data, GroundMesh.bounds);
        }

        public void OnRoomEnter()
        {
            if (CurrentState is RoomState.explored) { return; } // Already complete

            // Update room state
            switch (data.Type)
            {
                case RoomType.empty:
                    UpdateRoom(RoomState.explored);
                    break;
                case RoomType.combat:
                    UpdateRoom(RoomState.locked);
                    InitiateCombat();
                    break;
                default:
                    UpdateRoom(RoomState.undefined);
                    break;
            }
        }

        public void UpdateRoom(RoomState targetState = RoomState.undefined)
        {
            if (targetState != RoomState.undefined) { CurrentState = targetState; }
            foreach (var pair in ActiveDirectionDoorwayPairs)
            {
                pair.Value.UpdateState();
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
