using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]List<GameObject> spawnPoints = new List<GameObject>();

    [SerializeField]GameObject akashitaPrefab;
    [SerializeField]GameObject shutenDojiPrefab;

    void FixedUpdate()
    {
        // Temp debugging spawn key
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(Random.Range(0, 2) == 0 ? shutenDojiPrefab : akashitaPrefab,
            spawnPoint.transform.position + Vector3.up, Quaternion.identity);
    }
}
