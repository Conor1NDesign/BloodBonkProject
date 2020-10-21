using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[SerializeField]List<GameObject> enemies = new List<GameObject>();
    [SerializeField]List<GameObject> spawnPoints = new List<GameObject>();
	[SerializeField]int waveSize = 1;

#pragma warning disable 0649
    [SerializeField]GameObject akashitaPrefab;
    [SerializeField]GameObject shutenDojiPrefab;
#pragma warning restore 0649

	void FixedUpdate()
	{
			foreach (GameObject enemy in enemies)
			{
				IAttacker attacker = enemy.GetComponent<IAttacker>();
				if (attacker.currentDistanceToPlayer < attacker.range)
				{
					attacker.Attack();
				}
			}
			
			if (enemies.Count == 0)
				SpawnWave();
	}

    void SpawnWave()
    {
		for (int i = 0; i < waveSize; i++)
		{
			
		}
        foreach(GameObject spawnPoint in spawnPoints)
        {
			enemies.Add(SpawnEnemy(spawnPoint, Random.Range(0, 2) == 0 ? shutenDojiPrefab : akashitaPrefab));
        }
    }

    GameObject SpawnEnemy(GameObject spawnPoint, GameObject enemyPrefab)
    {
        return Instantiate(enemyPrefab, spawnPoint.transform.position + Vector3.up, Quaternion.identity);
    }
}