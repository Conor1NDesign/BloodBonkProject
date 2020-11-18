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

    [Range(0.01f, 1f)] [Tooltip("0.1 = Increase value by 10%")] 
    public float multiplier = 0.1f;

    // Classes
    PlayerStats stats;
    PlayerMovement movement;
    Weapon weapon;
    

    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        movement = FindObjectOfType<PlayerMovement>();
        weapon = FindObjectOfType<Weapon>();
    }

    // Apply powerup stat
    public void Pickup()
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
            if (weapon.attackSpeed < weapon.attackSpeedCap)
            {
                weapon.attackSpeed += weapon.attackSpeed * multiplier;

                if (weapon.attackSpeed > weapon.attackSpeedCap)
                {
                    weapon.attackSpeed = weapon.attackSpeedCap;
                }
            }
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
