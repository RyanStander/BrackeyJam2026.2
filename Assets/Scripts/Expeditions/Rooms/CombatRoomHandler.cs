using Factories;
using UnityEngine;

namespace Expeditions.Room {
    public class CombatRoomHandler : MonoBehaviour
    {
        private CombatRoomData roomData;
        private Bounds roomBounds;
        public EnemyFactory Factory;

        private Vector3 GetRandomPosition()
        {
            float random_x = Random.Range(-roomBounds.extents.x / 2f, roomBounds.extents.x / 2f);
            float random_z = Random.Range(-roomBounds.extents.z / 2f, roomBounds.extents.z / 2f);
            return roomBounds.center + new Vector3(random_x, roomBounds.center.y, random_z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, roomBounds.extents);
        }

        public void SpawnEnemiesTest(CombatRoomData data, Bounds bounds)
        {
            roomData = data;
            roomBounds = bounds;

            // TODO: Testing, spawn many
            for (int i = 0; i < data.SpawnBudget; i++)
            {
                Factory.CreateEnemy(EnemyType.Charger, GetRandomPosition());
            }
        }
    }
}
