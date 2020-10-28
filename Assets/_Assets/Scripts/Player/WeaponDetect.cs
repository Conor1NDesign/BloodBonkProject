using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    //public Score score;

    Weapon weapon;
    EnemyAI enemy;

    public Material redMat;
    public Material defaultMat;

    public float colourChangeDelay = 0.5f;
    float currentDelay = 0f;

    bool colourChangeCollision = false;

    void Start()
    {
        enemy = GetComponent<EnemyAI>();
        weapon = FindObjectOfType<Weapon>();
    }

    public void TakeDamage()
    {
        //score.UpdateScore();
        enemy.TakeDamage(weapon.damage);
        colourChangeCollision = true;
        currentDelay = Time.time + colourChangeDelay;
    }

    void Update()
    {
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
