using UnityEngine;

namespace Combat.Boss
{
    public class BossTelegraph : MonoBehaviour
    {
        [SerializeField] private GameObject circlePrefab;
        [SerializeField] private LineRenderer linePrefab;

        public void ShowCircle(Vector3 position, float radius, float duration)
        {
            GameObject indicator = Instantiate(circlePrefab, position, Quaternion.Euler(90f, 0f, 0f));
            indicator.transform.localScale = Vector3.one * radius * 2f;
            Destroy(indicator, duration);
        }

        public void ShowLine(Vector3 start, Vector3 direction, float length, float width, float duration)
        {
            LineRenderer line = Instantiate(linePrefab, start, Quaternion.identity);
            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, start + direction.normalized * length);
            line.startWidth = width;
            line.endWidth = width;
            Destroy(line.gameObject, duration);
        }
    }
}
