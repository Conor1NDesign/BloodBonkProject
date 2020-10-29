using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AvaliablePowerUp
{
    MaxHealth,
    MaxDash,
    AttackSpeed,
    Lifesteal,
    Damage
}

public class PowerUp : MonoBehaviour
{
    public AvaliablePowerUp currentPowerUp;
    public float multiplier = 0.1f;

    PlayerStats stats;
    PlayerMovement movement;
    Weapon weapon;

    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        movement = FindObjectOfType<PlayerMovement>();
        weapon = FindObjectOfType<Weapon>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        if (currentPowerUp == AvaliablePowerUp.MaxHealth)
        {
            stats.maxHealth += stats.maxHealth * multiplier;
        }
        else if (currentPowerUp == AvaliablePowerUp.MaxDash)
        {
            movement.maxDashMeter += movement.maxDashMeter * multiplier;
        }
        else if (currentPowerUp == AvaliablePowerUp.AttackSpeed)
        {
            weapon.attackSpeed += weapon.attackSpeed * multiplier;
            weapon.SetAttackSpeed(weapon.attackSpeed);
        }
        else if (currentPowerUp == AvaliablePowerUp.Lifesteal)
        {
            stats.lifesteal += multiplier;
        }
        else if (currentPowerUp == AvaliablePowerUp.Damage)
        {
            weapon.damage += weapon.damage * multiplier;
        }

        Destroy(gameObject);
    }
}
