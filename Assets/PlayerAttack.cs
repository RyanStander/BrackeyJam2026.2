using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float meleeAttackDamage = 10f;
    public Transform attackPoint;
    public float attackCooldown = 0.5f;
    public float attackRadius = 1f;
    public Vector3 attackOffset = new Vector3(0f, 0.5f, 0f); //this is probably really stupid

    private float nextAttackTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        Debug.Log("Melee attack performed!");
        DetectMeleeEnemies();
        nextAttackTime = Time.time + attackCooldown;
    }

    private void DetectMeleeEnemies()
    {
        if (attackPoint == null)
        {
            return;
        }

        Vector3 attackCenter = attackPoint.position + attackOffset;
        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter, attackRadius);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Hit " + enemy.name);
                enemy.GetComponentInParent<Health>()?.TakeDamage(meleeAttackDamage);
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position + attackOffset, attackRadius);
        Gizmos.color = Color.white;
    }
}