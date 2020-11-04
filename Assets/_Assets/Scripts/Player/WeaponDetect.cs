﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{

    // Classes
    EnemyAI enemy;
    Score score;

    // Debugging
    public Material redMat;
    public Material defaultMat;

    public float colourChangeDelay = 0.1f;
    float currentDelay = 0f;

    bool colourChangeCollision = false;

    void Start()
    {
        enemy = GetComponent<EnemyAI>();
        score = FindObjectOfType<Score>();
    }

    public void TakeDamage(float damage)
    {
        score.UpdateScore();
        enemy.TakeDamage(damage);
        //colourChangeCollision = true;
        //currentDelay = Time.time + colourChangeDelay;
    }

    void Update()
    {
        // Debugging
        //if (colourChangeCollision)
        //{
        //    transform.GetComponentInChildren<MeshRenderer>().material = redMat;
        //    if (Time.time > currentDelay)
        //    {
        //        transform.GetComponentInChildren<MeshRenderer>().material = defaultMat;
        //        colourChangeCollision = false;
        //    }
        //}
    }
}
