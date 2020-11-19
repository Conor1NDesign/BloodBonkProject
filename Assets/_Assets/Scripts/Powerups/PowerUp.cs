using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject notificationImage;
    public Color tempColor;

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

        if (currentPowerUp.ToString() == "MaxHealth")
            notificationImage = GameObject.Find("MaxHPUp");
        else if (currentPowerUp.ToString() == "AttackSpeed")
            notificationImage = GameObject.Find("AtkSpeedUp");
        else if (currentPowerUp.ToString() == "Lifesteal")
            notificationImage = GameObject.Find("LifestealUp");
        else if (currentPowerUp.ToString() == "MaxDash")
            notificationImage = GameObject.Find("MaxDashUp");
        else if (currentPowerUp.ToString() == "Damage")
            notificationImage = GameObject.Find("DamageUp");

        tempColor.a = 1f;
    }

    // Apply powerup stat
    public void Pickup()
    {
        notificationImage.GetComponent<Image>().color = tempColor;

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
