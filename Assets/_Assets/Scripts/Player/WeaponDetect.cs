using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    // Classes
    EnemyAI enemy;

    float currentDelay;
    bool canBeAttacked;

    void Start()
    {
        enemy = GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (currentDelay < Time.time)
        {
            canBeAttacked = true;
        }
        else
        {
            canBeAttacked = false;
        }
    }
        

    public void TakeDamage(float damage)
    {
        if (currentDelay < Time.time)
        {
            currentDelay = Time.time + 0.5f;

            enemy.TakeDamage(damage);
            

            Debug.Log("hit");
        }
    }
        
}
