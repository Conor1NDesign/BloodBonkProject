using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
	[Header("Difficulty Settings")]
	[Tooltip("1.0 = 100%")]public float difficulty = 0.9f;
	[SerializeField][Tooltip("Per Wave")]float difficultyIncreasePerWave = 0.05f;
	[SerializeField][Tooltip("Per Second")]float difficultyIncreasePerSecond = 0.0001f;

	[Header("Wave Settings")]
    [SerializeField]List<GameObject> spawnPoints = new List<GameObject>();
	[SerializeField]int baseWaveSize = 1;
	[SerializeField]int waveSize = 1;

#pragma warning disable 0649
	[Header("Prefabs")]
	[SerializeField]GameObject player;
    [SerializeField]GameObject akashitaPrefab;
    [SerializeField]GameObject shutenDojiPrefab;
#pragma warning restore 0649

	[Header("Current Enemy List")]
	[SerializeField]List<GameObject> enemies = new List<GameObject>();
	[HideInInspector]public List<AkashitaProjectile> akashitaProjectiles = new List<AkashitaProjectile>();

	// Classes
	SpawnPower powerupManager;
	Score score;

	void Start()
    {
		powerupManager = FindObjectOfType<SpawnPower>();
		score = FindObjectOfType<Score>();
    }

	void FixedUpdate()
	{
		difficulty += difficultyIncreasePerSecond / 60.0f;

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
			NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
			if (attacker.GetHealth() <= 0.0f)
			{
				enemies.Remove(enemy);
				Destroy(enemy);
				i--;

				// Increase score
				score.UpdateScore();

				// Chance to spawn rdm powerup
				powerupManager.SpawnPowerUp(enemy.transform.position);
			}
			if (attacker.currentDistanceToPlayer < attacker.range)
			{
				attacker.Attack();
				agent.updateRotation = false;
			}
			else
				agent.updateRotation = true;
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
			enemies[i].transform.position = spawnPoints[i % spawnPoints.Count].transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
			EnemyAI enemy = enemies[i].GetComponent<EnemyAI>();
			enemy.player = player;
			enemy.maxHealth = enemy.maxHealth * difficulty;
			enemy.health = enemy.maxHealth;
			enemy.damage = enemy.damage * difficulty;
			enemy.enemyManager = this;
		}
		waveSize = baseWaveSize * (int)difficulty;
		difficulty += difficultyIncreasePerWave;
    }

    GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        return Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}