using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public bool iFrameActive = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float damage)
    {
        if (iFrameActive)
        {
            return;
        }
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
    }
}
