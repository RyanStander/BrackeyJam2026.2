using Controllers;
using UI;
using UnityEngine;

namespace Combat.Stats
{
    public class CompanionGrievance : MonoBehaviour
    {
        #region Config

        public GrievanceDataSet GrievanceDataSet;

        [SerializeField] private float betrayalThreshold = 100f;

        [Header("Proximity Rule")]
        [SerializeField] private float proximityRadius = 5f;
        [SerializeField] private float proximityTickInterval = 0.5f;
        [SerializeField] private float proximityGrievancePerTick = 1f;
        [SerializeField] private LayerMask enemyLayerMask;

        [Header("Popup")]
        [SerializeField] private GameObject floatingTextPrefab;
        [SerializeField] private Vector3 popupOffset = new Vector3(0, 2f, 0);
        [SerializeField] private float popupSpawnRadius = 1f;

        [Header("Debug")]
        [SerializeField] private bool debugLogging = true;
        [SerializeField] private KeyCode debugAddKey = KeyCode.G;
        [SerializeField] private float debugAddAmount = 10f;

        #endregion

        #region State

        [SerializeField] private float grievance;

        private AIController controller;
        private bool hasBetrayed;
        private float proximityTimer;

        #endregion

        #region Public Accessors

        public float Grievance => grievance;
        public float BetrayalThreshold => betrayalThreshold;
        public float NormalizedGrievance => Mathf.Clamp01(grievance / betrayalThreshold);

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            controller = GetComponent<AIController>();
        }

        private void OnEnable() => AIController.OnEnemyDeath += HandleEnemyDeath;
        private void OnDisable() => AIController.OnEnemyDeath -= HandleEnemyDeath;

        private void Update()
        {
            HandleDebugInput();

            if (hasBetrayed) return;

            TickProximityGrievance();
        }

        #endregion

        #region Grievance Core

        public void AddGrievance(float amount)
        {
            if (hasBetrayed) return;

            float previous = grievance;
            grievance = Mathf.Max(0f, grievance + amount);

            if (!Mathf.Approximately(previous, grievance))
            {
                SpawnGrievancePopup(amount);

                if (debugLogging)
                    Debug.Log($"{gameObject.name} grievance: {previous:F2} -> {grievance:F2} (threshold {betrayalThreshold})");
            }

            if (grievance >= betrayalThreshold)
                Betray();
        }

        public void GiveLoot(int scrapValue)
        {
            AddGrievance(-scrapValue);
        }

        private void Betray()
        {
            hasBetrayed = true;

            if (debugLogging)
                Debug.Log($"{gameObject.name} has crossed the betrayal threshold!");

            controller.TriggerBetrayal(AIController.BetrayalType.Hostile);
        }

        #endregion

        #region Triggers

        private void HandleEnemyDeath(AIController enemy, bool wasExploited)
        {
            AddGrievance(!wasExploited ? GrievanceDataSet.FailedExploitGrievance : GrievanceDataSet.ExploitGrievance);
        }

        private void TickProximityGrievance()
        {
            proximityTimer += Time.deltaTime;
            if (proximityTimer < proximityTickInterval) return;

            proximityTimer = 0f;

            if (CheckForNearbyEnemy())
                AddGrievance(proximityGrievancePerTick);
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

        #endregion

        #region Feedback

        private void SpawnGrievancePopup(float amount)
        {
            if (floatingTextPrefab == null) return;

            Vector3 position = transform.position + popupOffset + Random.insideUnitSphere * popupSpawnRadius;

            GameObject popup = Instantiate(floatingTextPrefab, position, Quaternion.identity);
            string text = amount > 0 ? $"+{amount:F2}" : $"{amount:F2}";
            Color color = amount > 0 ? Color.red : Color.green; // grievance UP = bad = red, DOWN = good = green

            popup.GetComponent<FloatingText>().Setup(text, color);
        }

        #endregion

        #region Debug

        private void HandleDebugInput()
        {
            if (debugLogging && Input.GetKeyDown(debugAddKey))
                AddGrievance(debugAddAmount);
        }

        #endregion

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, proximityRadius);
        }
        #endif
    }
}
