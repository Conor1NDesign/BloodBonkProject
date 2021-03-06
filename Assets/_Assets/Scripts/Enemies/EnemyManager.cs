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
	[SerializeField][Tooltip("If enemies should be distributed evenly between all spawn points (true), or if they should spawn at random points (false)")]bool distributeEnemiesEvenly = true;
    [SerializeField]List<GameObject> spawnPoints = new List<GameObject>();
	[SerializeField]int baseWaveSize = 1;
	[SerializeField]int waveSize = 1;
	[SerializeField]int maxWaveSize = 1000000;

#pragma warning disable 0649
	[Header("Prefabs")]
	[SerializeField]GameObject player;
    [SerializeField]GameObject akashitaPrefab;
    [SerializeField]GameObject shutenDojiPrefab;
#pragma warning restore 0649

	[Header("Current Enemy Lists")]
	[SerializeField]List<GameObject> enemies = new List<GameObject>();
	[SerializeField]List<GameObject> ragdollingEnemies = new List<GameObject>();
	[SerializeField]List<GameObject> inactiveShutenDoji = new List<GameObject>();
	[SerializeField]List<GameObject> inactiveAkashita = new List<GameObject>();
	public List<AkashitaProjectile> akashitaProjectiles = new List<AkashitaProjectile>();

	// Classes
	Game gameManager;
	SpawnPower powerupManager;
	Score score;

	void Start()
    {
		powerupManager = FindObjectOfType<SpawnPower>();
		score = FindObjectOfType<Score>();
		gameManager = FindObjectOfType<Game>();
    }

	void Update()
	{
		// DEBUG: Debug commands activated with backslash
		if (Input.GetKey(KeyCode.Backslash))
		{
			// Damage all enemies
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				foreach(GameObject enemy in enemies)
					enemy.GetComponent<EnemyAI>().TakeDamage(10.0f);
			}
			// Kill all enemies
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				for (int i = 0; i < enemies.Count; i++)
				{
					enemies[i].GetComponent<EnemyAI>().health = -1.0f;
				}
			}
			// Spawn a wave
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				SpawnWave();
			}
			// God mode toggle
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				PlayerStats playerStats = player.GetComponent<PlayerStats>();
				playerStats.godMode = !playerStats.godMode;
			}
		}
	}
	
	void FixedUpdate()
	{
		difficulty += difficultyIncreasePerSecond / 60.0f;

		// Making enemies attack and die
		for (int i = 0; i < enemies.Count; i++)
		{
			GameObject enemy = enemies[i];
			EnemyAI attacker = enemy.GetComponent<EnemyAI>();
			NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
			if (attacker.GetHealth() <= 0.0f)
			{
				enemies.Remove(enemy);
				ragdollingEnemies.Add(enemy);
				attacker.Ragdoll();
				i--;

				// Increase score
				score.UpdateScore();
				gameManager.kills++;

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

		// Resets enemies that have been ragdolling for long enough
		// and returns them to the inactive pools
		for (int i = 0; i < ragdollingEnemies.Count; i++)
		{
			GameObject enemy = ragdollingEnemies[i];
			EnemyAI attacker = enemy.GetComponent<EnemyAI>();
			if (attacker.currentRagdollTime < 0.0f)
			{
				ragdollingEnemies.Remove(enemy);
				attacker.Unragdoll();
				enemy.SetActive(false);
				if (attacker is ShutenDojiAI)
					inactiveShutenDoji.Add(enemy);
				else
					inactiveAkashita.Add(enemy);
			}
		}
		
		// Clears old projectiles
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
		for (int i = 0; i < waveSize && i < maxWaveSize; i++)
		{
			GameObject newEnemy = SpawnEnemy(Random.Range(0, 2) == 0 ? shutenDojiPrefab : akashitaPrefab);
			enemies.Add(newEnemy);
			newEnemy.transform.position =
				spawnPoints[distributeEnemiesEvenly ?
					i % spawnPoints.Count :
					Random.Range(0, spawnPoints.Count)].transform.position +
				new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
			EnemyAI enemy = newEnemy.GetComponent<EnemyAI>();
			enemy.player = player;
			enemy.currentMaxHealth = enemy.baseMaxHealth * difficulty;
			enemy.health = enemy.currentMaxHealth;
			enemy.damage = enemy.baseDamage * difficulty;
			enemy.enemyManager = this;
		}
		waveSize = (int)(baseWaveSize * difficulty);
		difficulty += difficultyIncreasePerWave;
    }

    GameObject SpawnEnemy(GameObject enemyPrefab)
    {
		EnemyAI aiType = enemyPrefab.GetComponent<EnemyAI>();
		if (inactiveAkashita.Count == 0 && aiType is AkashitaAI ||
			inactiveShutenDoji.Count == 0 && aiType is ShutenDojiAI)
        	return Instantiate(enemyPrefab, transform.position, Quaternion.identity);
		else
		{
			GameObject enemyToReturn;
			// Grab an enemy
			if (aiType is ShutenDojiAI)
			{
				enemyToReturn = inactiveShutenDoji[inactiveShutenDoji.Count - 1];
				inactiveShutenDoji.RemoveAt(inactiveShutenDoji.Count - 1);
			}
			else
			{
				enemyToReturn = inactiveAkashita[inactiveAkashita.Count - 1];
				inactiveAkashita.RemoveAt(inactiveAkashita.Count - 1);
			}
			// Reset the enemy
			enemyToReturn.SetActive(true);
			EnemyAI ai = enemyToReturn.GetComponent<EnemyAI>();
			ai.health = ai.currentMaxHealth;
			ai.healthBar.SetMaxHealth(100);
			ai.healthBar.SetHealth(100);
			return enemyToReturn;
		}
    }
}