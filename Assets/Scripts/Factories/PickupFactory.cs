using UnityEngine;

namespace Factories
{
    public class PickupFactory : MonoBehaviour
    {
        //TODO: We may want to make a generic pickup and add values to modify how it looks
        public GameObject ScrapPrefab;
        
        public GameObject CreatePickup(PickupType type, Vector3 position)
        {
            GameObject prefab = type switch
            {
                PickupType.Scrap => ScrapPrefab,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            GameObject pickup = Instantiate(prefab, position, Quaternion.identity);
            return pickup;
        }
    }
}
