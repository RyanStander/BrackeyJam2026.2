using System;
using Controllers;
using UnityEngine;

namespace Combat.Stats
{
    public class CompanionGrievance : MonoBehaviour
    {
        public GrievanceDataSet GrievanceDataSet;

        [SerializeField] private float grievance;
        [SerializeField] private float betrayalThreshold = 100f;

        [Header("Proximity Rule")] [SerializeField]
        private float proximityRadius = 5f;

        [SerializeField] private LayerMask enemyLayerMask;

        [Header("Debug")] [SerializeField] private bool debugLogging = true;
        [SerializeField] private KeyCode debugAddKey = KeyCode.G;
        [SerializeField] private float debugAddAmount = 10f;

        private AIController controller;
        private bool hasBetrayed;

        public float Grievance => grievance;
        public float BetrayalThreshold => betrayalThreshold;
        public float NormalizedGrievance => Mathf.Clamp01(grievance / betrayalThreshold);

        private void Awake()
        {
            controller = GetComponent<AIController>();
        }

        private void OnEnable() => AIController.OnEnemyDeath += HandleEnemyDeath;
        private void OnDisable() => AIController.OnEnemyDeath -= HandleEnemyDeath;

        private void HandleEnemyDeath(AIController enemy, bool wasExploited)
        {
            if (!wasExploited)
                AddGrievance(GrievanceDataSet.FailedExploitGrievance);
        }

        public void AddGrievance(float amount)
        {
            if (hasBetrayed) return;

            float previous = grievance;
            grievance = Mathf.Max(0f, grievance + amount);

            if (debugLogging && !Mathf.Approximately(previous, grievance))
                Debug.Log(
                    $"{gameObject.name} grievance: {previous:F1} -> {grievance:F1} (threshold {betrayalThreshold})");

            if (grievance >= betrayalThreshold)
                Betray();
        }

        private void Betray()
        {
            hasBetrayed = true;
            if (debugLogging)
                Debug.Log($"{gameObject.name} has crossed the betrayal threshold!");

            controller.TriggerBetrayal(AIController.BetrayalType.Hostile);
        }

        private void Update()
        {
            if (debugLogging && Input.GetKeyDown(debugAddKey))
            {
                AddGrievance(debugAddAmount);
            }

            if (hasBetrayed) return;

            bool enemyNearby = CheckForNearbyEnemy();
            if (enemyNearby)
                AddGrievance(GrievanceDataSet.ProximityGrievance * Time.deltaTime);
        }
        
        private bool CheckForNearbyEnemy()
        {
            Collider[] nearby = Physics.OverlapSphere(transform.position, proximityRadius, enemyLayerMask);
            foreach (Collider col in nearby)
            {
                if (col.gameObject.CompareTag(controller.TargetTag))
                    return true;
            }
            return false;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, proximityRadius);
        }
        #endif

        public void GiveLoot(int scrapValue)
        {
            AddGrievance(-scrapValue);
        }
    }
}
