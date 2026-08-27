using AYellowpaper.SerializedCollections;
using Factories;
using System.Collections.Generic;
using UnityEngine;

namespace Expeditions.Room
{
    [CreateAssetMenu(fileName = "CombatRoomData", menuName = "Expeditions/Rooms/CombatData")]
    public class CombatRoomData : RoomData
    {
        public override RoomType Type => RoomType.combat;

        [SerializedDictionary("Enemy", "Weight")]
        public SerializedDictionary<EnemyType, float> EnemyWeightDistributionPair = new();

        [field: SerializeField]
        public int SpawnBudget { get; private set; }

        [field: SerializeField]
        public int SpawnFunds { get; private set; }
    }
}