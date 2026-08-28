
using Factories;
using UnityEngine;

namespace Arena
{
    public class SpawnPoint : MonoBehaviour
    {
        [field: SerializeField]
        public SpawnPointType Type { get; private set; }

        [field: SerializeField]
        public EnemyType[] Enemies { get; private set; }
    }
}