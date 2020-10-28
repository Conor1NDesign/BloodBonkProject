using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float lifesteal = 1.1f;

    private float currentHealth;

    public Material redMat;
    public Material defaultMat;

    public float colourChangeDelay = 0.5f;
    float currentDelay = 0f;

    bool colourChangeCollision = false;

    // Classes
    public HealthBar health;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentDelay = Time.time + colourChangeDelay;

        if (currentHealth <= 0f)
        {
            Debug.Log("You ded");
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        health.SetHealth(currentHealth);

        if (colourChangeCollision)
        {
            transform.GetComponentInChildren<MeshRenderer>().material = redMat;
            if (Time.time > currentDelay)
            {
                transform.GetComponentInChildren<MeshRenderer>().material = defaultMat;
                colourChangeCollision = false;
            }
        }
    }
}
