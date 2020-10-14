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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    void Pickup()
    {

        Destroy(gameObject);
    }
}
