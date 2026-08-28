using AYellowpaper.SerializedCollections;
using Factories;
using System.Collections.Generic;
using UnityEngine;
using EventType = Events.EventType;

namespace Arena.Wave
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "Arena/Wave/Data")]
    public class WaveData : ScriptableObject
    {
        [SerializedDictionary("Type", "Count")]
        public SerializedDictionary<EnemyType, int> EnemyCount = new();

        [field: SerializeField, Tooltip("Use to limit how many enemies from this wave can be present at once, if 0 uncapped.")]
        public int SpawnLimit { get; private set; } = 0;

        // Doesnt do anything yet, but could be used to define events
        [field: SerializeField, Tooltip("Events (other than OnWaveStart) that should fire at the start of a wave.")]
        public List<EventType> OnStartEvents { get; private set;} = new();

        [field: SerializeField, Tooltip("Events (other than OnWaveEnd) that should fire at the end of a wave")]
        public List<EventType> OnEndEvents { get; private set; } = new();
    }
}
