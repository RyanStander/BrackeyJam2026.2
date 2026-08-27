using UnityEngine;

namespace Combat.Stats
{
    [CreateAssetMenu(fileName = "GrievanceData", menuName = "Grievance", order = 0)]
    public class GrievanceDataSet : ScriptableObject
    {
        [Header("Specific Grievance Values")]
        public float ExploitGrievance = -3f;
        public float FailedExploitGrievance = 1f;

        public float ProximityGrievance = 1f;
    }
}
