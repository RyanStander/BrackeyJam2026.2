using Combat.Stats;
using UnityEngine;

// Class should be deleted, it's just in for testing purposes
public class BasicCharacterController : MonoBehaviour
{
    public CharacterController controller;

    [Range((float) 0.25, 1)]
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(direction * speed);

        if (Input.GetKeyUp(KeyCode.Space)) {


            Health[] healths = FindObjectsOfType<Health>();

            foreach (Health health in healths)
            {
                health.TakeDamage(new Combat.Data.DamageInfo(50f, Combat.Data.Faction.Allies, null));
            }

        }

        
    }
}
