using System;
using Unity.AI.Navigation;
using UnityEngine;
namespace Movement
{
    public class ArenaNavBaker : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface surface;

        private void Awake()
        {
            RebakeNavMesh();
        }

        public void RebakeNavMesh()
        {
            surface.BuildNavMesh();
        }
    }
}
