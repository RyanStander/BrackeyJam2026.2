using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float meleeAttackDamage = 10f;
    public float shootAttackDamage = 10f;
    public Transform attackPoint;
    public float attackCooldown = 0.5f;
    public float attackRadius = 1f;
    public Vector3 attackOffset = new Vector3(0f, 0.5f, 0f); //this is probably really stupid
    public GameObject bulletPrefab;
    public GameObject meleePrefab;

    public PlayerMovement playerMovement;

    private float nextAttackTime;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        meleePrefab.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            MeleeAttack();
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            ShootAttack();
        }

    }

    private void MeleeAttack()
    {
        Debug.Log("Melee attack performed!");
        DetectMeleeEnemies();
        //Lunge();
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
                enemy.GetComponent<Health>()?.TakeDamage(meleeAttackDamage);
            }
        }
    }

    public void ShootAttack()
    {
        DetectShootEnemies();
    }

    private void DetectShootEnemies()
    {
        Vector3 endPoint = attackPoint.position + transform.up * 40f;
        if (Physics.Raycast(attackPoint.position, transform.up , out RaycastHit hit, 40f)) //ok right now i use up for testing since we're flat arrow, just change to forward with proper character!
        {
            endPoint = hit.point;
            hit.collider.GetComponent<Health>()?.TakeDamage(shootAttackDamage);
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log(hit.collider.gameObject.name);
        }
        Debug.DrawRay(attackPoint.position, endPoint);
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