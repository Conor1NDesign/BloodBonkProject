﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPower : MonoBehaviour
{
    public GameObject[] powerUps;

    [Range(0f, 100f)] public float powerUpSpawnChance = 100f;

    public void SpawnPowerUp(Vector3 position)
    {
        if (powerUpSpawnChance < Random.Range(0f, 100f))
        {
            int num = Random.Range(0, 4);

            position.y += 0.5f;

            Instantiate(powerUps[num], position, Quaternion.identity);
        }
    }
}
