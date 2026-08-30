
using Factories;
using UnityEngine;

namespace Arena
{
    public partial class SpawnPoint : MonoBehaviour
    {
        [field: SerializeField]
        public SortFilterType Type { get; private set; }

        [field: SerializeField]
        public EnemyType[] EnemyTypes { get; private set; }

        [field: SerializeField]
        public SpawnPointShape Shape { get; private set; }

        public CapsuleCollider CapsuleCollider { get; private set; }
        public BoxCollider BoxCollider { get; private set; }

        private void HandleShape()
        {
            switch (Shape)
            {
                case SpawnPointShape.Circle:
                    BoxCollider.gameObject.SetActive(false);
                    CapsuleCollider.gameObject.SetActive(true);
                    break;
                case SpawnPointShape.Rectangle:
                    BoxCollider.gameObject.SetActive(true);
                    CapsuleCollider.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private void OnValidate()
        {
            if (CapsuleCollider == null) CapsuleCollider = GetComponentInChildren<CapsuleCollider>(true);
            if (BoxCollider == null) BoxCollider = GetComponentInChildren<BoxCollider>(true);
            HandleShape();
        }
    }
}