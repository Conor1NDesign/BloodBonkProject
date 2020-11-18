using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{

    // Classes
    EnemyAI enemy;


    void Start()
    {
        enemy = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float damage)
    {
        enemy.TakeDamage(damage);
    }
}
