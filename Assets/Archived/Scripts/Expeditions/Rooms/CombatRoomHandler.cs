using Events;
using Factories;
using UnityEngine;
using EventType = Events.EventType;

namespace Expeditions.Room {
    public class CombatRoomHandler : MonoBehaviour
    {
        private CombatRoomData roomData;
        private RoomManager currentRoom;
        private Bounds roomBounds;

        private bool isRunning = false;

        private int currentFunds = 0;
        private int currentEnemies = 0;

        [Range(0.1f, 2)]
        public float spawnInterval = 0.5f;
        public float spawnTimer = 0;

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.ExecuteCombatRoom, OnExecuteCombatRoom);
            EventManager.currentManager.Subscribe(EventType.OnEnemyDeath, OnEnemyDeath);
        }
        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.ExecuteCombatRoom, OnExecuteCombatRoom);
            EventManager.currentManager.Unsubscribe(EventType.OnEnemyDeath, OnEnemyDeath);
        }

        private void OnEnemyDeath(EventData eventData)
        {
            // TODO: Include data about what room the enemy died in.
            if (!eventData.IsEventOfType(out OnEnemyDeath command)) return;

            currentEnemies--;

            if (currentEnemies <= 0 && currentFunds <= 0 && isRunning)
            {
                // TODO: Create room method for completion once there are room rewards defined in (room data)
                currentRoom.UpdateRoom(RoomState.explored);
                isRunning = false;
            }
        }

        private void OnExecuteCombatRoom(EventData eventData)
        {
            if (!eventData.IsEventOfType(out ExecuteCombatRoom command)) return;

            currentRoom = command.SourceRoom;
            roomBounds = command.RoomBounds;
            roomData = command.RoomData;

            currentFunds = roomData.SpawnFunds;
            spawnTimer = 0;
            isRunning = true;
        }

        private void TrySpawnEnemy()
        {
            if (currentEnemies < roomData.SpawnBudget && currentFunds > 0)
            {
                currentEnemies++;
                currentFunds--;

                // TODO: Fix method to actually use the weight distribution and perhaps have boss priority. Alternatively, create BossRoomData.
                if (roomData.EnemyWeightDistributionPair.ContainsKey(EnemyType.MooBoss))
                {
                    EventManager.currentManager.AddEvent(new CreateEnemy(EnemyType.MooBoss, GetRandomPosition()));
                    currentFunds = 0;
                    currentEnemies = 1;
                    return;
                }

                // TODO: Implement weights and proper costs, for now just randomly pick between enemy types with very ugly logic.
                EnemyType enemyType = (EnemyType) Random.Range((int)EnemyType.Charger, (int)EnemyType.Swatter + 1);
                EventManager.currentManager.AddEvent(new CreateEnemy(enemyType, GetRandomPosition()));
            }
        }

        private void Update()
        {
            if (!isRunning) return;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                TrySpawnEnemy();
            }
        }

        private Vector3 GetRandomPosition()
        {
            float random_x = Random.Range(-roomBounds.extents.x / 2f, roomBounds.extents.x / 2f);
            float random_z = Random.Range(-roomBounds.extents.z / 2f, roomBounds.extents.z / 2f);
            return roomBounds.center + new Vector3(random_x, roomBounds.center.y + 1.5f, random_z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, roomBounds.extents);
        }
    }
}
