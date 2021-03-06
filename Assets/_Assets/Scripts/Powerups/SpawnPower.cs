﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPower : MonoBehaviour
{
    public GameObject[] powerUps;

    [Range(0f, 100f)] public float spawnChance = 100f;

    [Header("PowerUp Sounds")]
    public AudioSource pickUpSound;

    public void SpawnPowerUp(Vector3 position)
    {
        if (spawnChance > Random.Range(0f, 100f))
        {
            int num = Random.Range(0, 5);

            Instantiate(powerUps[num], position, Quaternion.identity);
        }
    }
}
