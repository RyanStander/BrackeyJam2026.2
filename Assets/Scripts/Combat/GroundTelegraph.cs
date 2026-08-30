using UnityEngine;

namespace Combat
{
    public class GroundTelegraph : MonoBehaviour
    {
        [SerializeField] private GameObject telegraphPrefab;

        public GameObject ShowCircle(Vector3 position, float radius, float duration)
        {
            GameObject indicator = Instantiate(telegraphPrefab, position, Quaternion.identity);
            indicator.transform.localScale = Vector3.one * radius * 2f;
            Destroy(indicator, duration);
            return indicator;
        }
    }
}
