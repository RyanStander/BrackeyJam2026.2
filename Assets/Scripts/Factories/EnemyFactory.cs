using System;
using UnityEngine;

namespace Factories
{
    public class EnemyFactory : MonoBehaviour
    {
        public GameObject ChargerPrefab;
        public GameObject SwatterPrefab;
        public GameObject MooBossPrefab;
        public GameObject CreateEnemy(EnemyType type, Vector3 position)
        {
            GameObject prefab = type switch
            {
                EnemyType.Charger => ChargerPrefab,
                EnemyType.Swatter => SwatterPrefab,
                EnemyType.MooBoss => MooBossPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
            return enemy;
        }
    }
}
