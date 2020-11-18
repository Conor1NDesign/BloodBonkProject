using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    // Classes
    EnemyAI enemy;

    float currentDelay;

    void Start()
    {
        enemy = GetComponent<EnemyAI>();
    }      

    public void TakeDamage(float damage)
    {
        if (currentDelay < Time.time)
        {
            currentDelay = Time.time + 0.5f; // Enemy invincible for 0.5secs

            enemy.TakeDamage(damage);
        }
    }
        
}
