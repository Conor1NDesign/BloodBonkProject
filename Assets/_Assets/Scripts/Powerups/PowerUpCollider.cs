using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollider : MonoBehaviour
{
    PowerUp powerUp;

    void Start()
    {
        powerUp = GetComponentInParent<PowerUp>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            powerUp.Pickup();
        }
    }
}
