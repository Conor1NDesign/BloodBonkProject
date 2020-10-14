using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]GameObject enemyPrefab;

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
        Instantiate(enemyPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}
