using Arena.Wave;
using Events;
using Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using EventType = Events.EventType;
using Random = UnityEngine.Random;

namespace Arena
{
    public class ArenaCombatHelper : MonoBehaviour
    {
        private readonly Queue<WaveData> WaveQueue = new();
        private WaveData CurrentWave = null;
        private bool IsRunning = false;
        private bool IsWaveRunning = false;

        private float WaveDelay = 0;
        private float WaveDelayTimer = 0;

        private readonly Queue<EnemyType> PendingEnemies = new();
        private int ActiveEnemies = 0;

        [field: SerializeField]
        public SpawnPoint[] SpawnPoints { get; private set; }
        private readonly Dictionary<EnemyType, List<SpawnPoint>> SortedSpawns = new();

        [field: SerializeField, Range(0.1f, 2)]
        private float SpawnInterval = 0.5f;
        private float SpawnTimer = 0;

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.OnEnemyDeath, OnEnemyDeath);
        }
        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.OnEnemyDeath, OnEnemyDeath);
        }

        private void Start()
        {
            SortSpawnPoints();
        }

        public void SortSpawnPoints()
        {
            foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
            {
                SortedSpawns[type] = new();
                foreach (SpawnPoint point in SpawnPoints)
                {
                    bool contains = point.EnemyTypes.Contains(type);
                    switch (point.Type)
                    {
                        case SortFilterType.Undefined:
                            SortedSpawns[type].Add(point);
                            break;
                        case SortFilterType.Whitelist:
                            if (contains) SortedSpawns[type].Add(point);
                            break;
                        case SortFilterType.Blacklist:
                            if (!contains) SortedSpawns[type].Add(point);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void RunWaveEvents(ICollection<EventType> events)
        {
            foreach (EventType type in events)
            {
                switch (type)
                {
                    case EventType.BossStart:
                        break;
                    case EventType.BossEnd:
                        break;
                    default:
                        break;
                }
            }
        }

        private void CheckForCompletion()
        {
            if (WaveQueue.Count == 0)
            {
                EventManager.currentManager.AddEvent(new WavesCompleted());
                GameState.CurrentArea++;
                ResetHelper();
            }
        }

        private void WaveEnd()
        {
            EventManager.currentManager.AddEvent(new WaveEnd());
            RunWaveEvents(CurrentWave.OnEndEvents);

            CurrentWave = null;
            IsWaveRunning = false;
            WaveDelayTimer = 0;
            CheckForCompletion();
        }

        private void WaveStart()
        {
            IsWaveRunning = true;
            EventManager.currentManager.AddEvent(new WaveStart());
            RunWaveEvents(CurrentWave.OnStartEvents);
        }

        private void HandleNextWave()
        {
            WaveDelayTimer += Time.deltaTime;
            if (WaveDelayTimer < WaveDelay) return;

            CurrentWave = WaveQueue.Dequeue();

            List<EnemyType> enemies = new();
            foreach (KeyValuePair<EnemyType, int> pair in CurrentWave.EnemyCount)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    enemies.Add(pair.Key);
                }
            }
            enemies.Shuffle();

            foreach (EnemyType type in enemies)
            {
                PendingEnemies.Enqueue(type);
            }
            WaveStart();
        }

        private void OnEnemyDeath(EventData eventData)
        {
            if (!eventData.IsEventOfType(out OnEnemyDeath command)) return;

            ActiveEnemies--;
            if (ActiveEnemies == 0 && PendingEnemies.Count == 0 && IsRunning)
            {
                WaveEnd();
            }
        }

        private Vector3 GetSpawnPointFiltered(EnemyType type)
        {
            SpawnPoint point;
            if (SortedSpawns[type].Count > 0)
            {
                point = SortedSpawns[type][Random.Range(0, SortedSpawns[type].Count)];
            }
            else // Should probably add an extra failcase for no spawnpoints in scene
            {
                point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
                Debug.LogError("Failed to find spawn point of type " + type + ", defaulting to random spawner.");
            }

            Vector3 pos = point.transform.position;
            Bounds bounds;
            float random_x;
            float random_z;
            // Tired brain, use just the same methodology for now.
            switch (point.Shape)
            {
                default:
                    bounds = point.BoxCollider.bounds;
                    random_x = Random.Range(-bounds.extents.x / 2f, bounds.extents.x / 2f);
                    random_z = Random.Range(-bounds.extents.z / 2f, bounds.extents.z / 2f);
                    pos = bounds.center + new Vector3(random_x, bounds.center.y, random_z);
                    break;
            }

            switch (type)
            {
                case EnemyType.MooBoss:
                    pos.y += 4;
                    break;
                default:
                    pos.y += 1.75f;
                    break;
            }
            return pos;
        }

        private void TrySpawnEnemy()
        {
            if (((CurrentWave.SpawnLimit == 0) || (ActiveEnemies < CurrentWave.SpawnLimit)) && PendingEnemies.Count != 0)
            {
                ActiveEnemies++;
                EnemyType RequestType = PendingEnemies.Dequeue();
                EventManager.currentManager.AddEvent(new CreateEnemy(RequestType, GetSpawnPointFiltered(RequestType)));
            }
        }

        private void UpdateWave(){
            SpawnTimer += Time.deltaTime;
            if (SpawnTimer >= SpawnInterval)
            {
                SpawnTimer = 0f;
                TrySpawnEnemy();
            }
        }

        private void Update()
        {
            if (!IsRunning) return;

            if (!CurrentWave || !IsWaveRunning) HandleNextWave();

            if (!IsWaveRunning) return;

            UpdateWave();
        }

        private void ResetHelper()
        {
            WaveDelayTimer = 0;

            PendingEnemies.Clear();
            ActiveEnemies = 0;

            CurrentWave = null;
            WaveQueue.Clear();
            IsRunning = false;
            IsWaveRunning = false;
        }

        public void RunHelper(WaveDataSet set, int waveDelay = 0)
        {
            WaveDelay = waveDelay;
            ResetHelper();
            foreach (WaveData wave in set.Waves)
            {
                WaveQueue.Enqueue(wave);
            }
            IsRunning = true;
        }
    }
}
