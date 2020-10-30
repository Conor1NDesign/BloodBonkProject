using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPower : MonoBehaviour
{
    public GameObject[] power;

    public void SpawnPowerUp(Vector3 position)
    {
        int num = Random.Range(0, 4);

        position.y += 0.5f;

        Instantiate(power[num], position, Quaternion.identity);
    }
}
