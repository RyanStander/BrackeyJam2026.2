using Arena.Wave;
using UnityEngine;

namespace Arena.Wave
{
    [CreateAssetMenu(fileName = "WaveDataSet", menuName = "Arena/Wave/Set")]
    public class WaveDataSet : ScriptableObject
    {
        [field:SerializeField]
        public WaveData[] Waves { get; private set; }
    }
}
