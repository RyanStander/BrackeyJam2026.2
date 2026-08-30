using Events;
using UnityEngine;

namespace Expeditions
{
    public class AreaExit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.currentManager.AddEvent(new ReturnToHub());
            }
        }
    }
}