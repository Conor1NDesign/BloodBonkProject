using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float lifesteal = 0.1f;

    private float currentHealth;

    // Classes
    HealthBar health;
    Menu menu;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
            TakeDamage(10.0f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            menu.toggleMenu();
        }
    }

    void Start()
    {
        health = FindObjectOfType<HealthBar>();
        menu = FindObjectOfType<Menu>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        health.SetHealth(currentHealth);
        health.SetMaxHealth(maxHealth);
    }
}
