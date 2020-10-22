using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[SerializeField]List<GameObject> enemies = new List<GameObject>();
    [SerializeField]List<GameObject> spawnPoints = new List<GameObject>();
	[SerializeField]int waveSize = 1;

#pragma warning disable 0649
	[SerializeField]GameObject player;
    [SerializeField]GameObject akashitaPrefab;
    [SerializeField]GameObject shutenDojiPrefab;
#pragma warning restore 0649
	void Start()
	{
		// temp
		foreach (GameObject enemy in enemies)
		{
			enemy.GetComponent<IAttacker>().player = player;
		}
	}	

	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Backslash))
		{
			foreach(GameObject enemy in enemies)
			Destroy(enemy);
			enemies.Clear();
		}

		foreach (GameObject enemy in enemies)
		{
			IAttacker attacker = enemy.GetComponent<IAttacker>();
			if (attacker.currentDistanceToPlayer < attacker.range)
				attacker.Attack();
		}
		
		if (enemies.Count == 0)
			SpawnWave();
	}

    void SpawnWave()
    {
		for (int i = 0; i < waveSize; i++)
		{
			enemies.Add(SpawnEnemy(Random.Range(0, 2) == 0 ? shutenDojiPrefab : akashitaPrefab));
			enemies[i].transform.position = spawnPoints[i % spawnPoints.Count].transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 1.0f, Random.Range(-1.0f, 1.0f));
			enemies[i].GetComponent<IAttacker>().player = player;
		}
    }

    GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        return Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}