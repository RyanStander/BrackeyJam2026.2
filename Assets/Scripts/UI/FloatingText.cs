using TMPro;
using UnityEngine;

namespace UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private float riseSpeed = 1f;
        [SerializeField] private float lifetime = 1f;

        private float timer;

        public void Setup(string text, Color color)
        {
            label.text = text;
            label.color = color;
        }

        private void Update()
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            timer += Time.deltaTime;

            if (timer >= lifetime)
                Destroy(gameObject);
        }
    }
}
