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
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        foreach(GameObject spawnPoint in spawnPoints)
        {
            SpawnEnemy(spawnPoint, Random.Range(0, 2) == 0 ? shutenDojiPrefab : akashitaPrefab);
        }
    }

    void SpawnEnemy(GameObject spawnPoint, GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoint.transform.position + Vector3.up, Quaternion.identity);
    }
}
