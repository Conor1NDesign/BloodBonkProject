using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float lifesteal = 1.1f;

    private float currentHealth;

    // Classes
    public HealthBar health;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        health.SetHealth(currentHealth);
    }

    public void LifestealDamage()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Debug.Log("You ded");
        }
    }
}
