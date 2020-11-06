﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    [Tooltip("0.1 = Heal 10% of damage")] public float lifesteal = 0.1f;

    private float currentHealth;

    // Classes
    HealthBar health;
    Game gameManager;

    // Enemy attacks player
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
            TakeDamage(10.0f);
        if (other.gameObject.CompareTag("Projectile"))
		{
			TakeDamage(10.0f);
			Destroy(other.transform.root.gameObject);
		}
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            // Enable Game Over Menu
            gameManager.GameOver();
        }
    }

    public void Lifesteal(float damage)
    {
        currentHealth += damage * lifesteal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void Start()
    {
        health = FindObjectOfType<HealthBar>();
        gameManager = FindObjectOfType<Game>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Update Health UI
        health.SetHealth(currentHealth);
        health.SetMaxHealth(maxHealth);
    }
}
