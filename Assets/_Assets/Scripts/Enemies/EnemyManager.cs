using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[HideInInspector]public float difficulty = 1.0f;
	[HideInInspector]public List<AkashitaProjectile> akashitaProjectiles = new List<AkashitaProjectile>();

	[SerializeField]List<GameObject> enemies = new List<GameObject>();
    [SerializeField]List<GameObject> spawnPoints = new List<GameObject>();
	[SerializeField]int baseWaveSize = 1;
	[SerializeField]int waveSize = 1;

#pragma warning disable 0649
	[SerializeField]GameObject player;
    [SerializeField]GameObject akashitaPrefab;
    [SerializeField]GameObject shutenDojiPrefab;
#pragma warning restore 0649
	void FixedUpdate()
	{
		difficulty += 0.001f / 60.0f;

		// DEBUG: Damage enemies with backslash
		if (Input.GetKeyDown(KeyCode.Backslash))
		{
			foreach(GameObject enemy in enemies)
			enemy.GetComponent<EnemyAI>().TakeDamage(10.0f);
		}

		// Making enemies attack and die
		for (int i = 0; i < enemies.Count; i++)
		{
			GameObject enemy = enemies[i];
			EnemyAI attacker = enemy.GetComponent<EnemyAI>();
			if (attacker.GetHealth() <= 0.0f)
			{
				enemies.Remove(enemy);
				Destroy(enemy);
				i--;
			}
			if (attacker.currentDistanceToPlayer < attacker.range)
				attacker.Attack();
		}
		
		for (int i = 0; i < akashitaProjectiles.Count; i++)
			if (akashitaProjectiles[i].timeLeft <= 0)
			{
				Destroy(akashitaProjectiles[i].gameObject);
				akashitaProjectiles.RemoveAt(i);
				i--;
			}

		if (enemies.Count == 0)
			SpawnWave();
	}

	// Spawns a wave of enemies, distributed throughout the spawn points
    void SpawnWave()
    {
		for (int i = 0; i < waveSize; i++)
		{
			enemies.Add(SpawnEnemy(Random.Range(0, 2) == 0 ? shutenDojiPrefab : akashitaPrefab));
			enemies[i].transform.position = spawnPoints[i % spawnPoints.Count].transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 1.0f, Random.Range(-1.0f, 1.0f));
			EnemyAI enemy = enemies[i].GetComponent<EnemyAI>();
			enemy.player = player;
			enemy.maxHealth = enemy.maxHealth * difficulty;
			enemy.damage = enemy.damage * difficulty;
			enemy.enemyManager = this;
		}
		waveSize = baseWaveSize * (int)difficulty;
		difficulty += 0.05f;
    }

    GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        return Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}