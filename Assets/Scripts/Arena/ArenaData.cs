using Arena.Wave;
using UnityEngine;

namespace Arena
{
    [CreateAssetMenu(fileName = "ArenaData", menuName = "Arena/ArenaData")]
    public class ArenaData : ScriptableObject
    {
        [field: SerializeField]
        public WaveDataSet Waves { get; private set; }

        [field: SerializeField, Range(0, 5)]
        public int WaveDelay { get; private set; } = 5;
    }
}
