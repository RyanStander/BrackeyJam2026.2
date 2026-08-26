using System;
using UnityEngine;

namespace Factories
{
    public class EnemyFactory : MonoBehaviour
    {
        public GameObject ChargerPrefab;
        
        public GameObject CreateEnemy(EnemyType type, Vector3 position)
        {
            GameObject prefab = type switch
            {
                EnemyType.Charger => ChargerPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
            return enemy;
        }
    }
}
