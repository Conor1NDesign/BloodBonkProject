using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    [Tooltip("0.1 = Heal 10% of damage")] public float lifesteal = 0.1f;

    private float currentHealth;

	[HideInInspector]public bool godMode = false;

    // Classes
    HealthBar health;
    Game gameManager;

    // Enemy attacks player
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
		{
			TakeDamage(other.transform.root.gameObject.GetComponent<AkashitaProjectile>().damage);
			Destroy(other.transform.root.gameObject);
		}
    }

    public void TakeDamage(float damage)
    {
		if (godMode)
			return;
		
        currentHealth -= damage;
        gameManager.dmgReceived += damage;

        if (currentHealth <= 0f)
        {
            // Plays Death animation
            gameManager.DeathAnimation();
        }
    }

    public void Lifesteal(float damage)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += damage * lifesteal;
            gameManager.lifeStolen += damage * lifesteal;
        }

        if (currentHealth > maxHealth)
        {
            gameManager.lifeStolen -= currentHealth - maxHealth;

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
