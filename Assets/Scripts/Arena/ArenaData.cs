using Arena.Wave;
using AYellowpaper.SerializedCollections;
using Factories;
using UnityEngine;

namespace Arena
{
    [CreateAssetMenu(fileName = "ArenaData", menuName = "Arena/ArenaData")]
    public class ArenaData : ScriptableObject
    {
        [field: SerializeField]
        public WaveDataSet Waves { get; private set; }

    }
}
