using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollider : MonoBehaviour
{
    // Classes
    SpawnPower powerUpManager;
    PowerUp powerUp;

    void Start()
    {
        powerUp = GetComponentInParent<PowerUp>();
        powerUpManager = FindObjectOfType<SpawnPower>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (powerUpManager.pickUpSound != null)
                powerUpManager.pickUpSound.Play();

            powerUp.Pickup();
        }
    }
}
