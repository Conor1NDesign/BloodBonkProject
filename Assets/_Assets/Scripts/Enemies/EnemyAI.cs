using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	[HideInInspector]public float currentDistanceToPlayer = 10.0f;
	[HideInInspector]public EnemyManager enemyManager;
	public GameObject player;
	public NavMeshAgent agent;
	
	public float range = 1.0f;
	[SerializeField]protected float timeBetweenAttacks = 1.0f;
	//HEALTH!
	public float maxHealth = 100.0f;
	[HideInInspector]public float health = 100.0f;
	public float damage = 10.0f;
	
	// The rigidbody of the enemy
	Rigidbody enemyRigidBody;
	protected float timeToNextAttack = 0.0f;

	public void TakeDamage(float damage)
	{
		health -= damage;
	}

	public float GetHealth()
	{
		return health;
	}
	
	void Awake()
	{
		// Cache this GameObject's rigidbody and navmesh agent
		enemyRigidBody = GetComponent<Rigidbody>();
		agent = GetComponent<NavMeshAgent>();
		health = maxHealth;
	}

	void FixedUpdate()
	{
		if (timeToNextAttack > 0.0f)
		{
			timeToNextAttack -= 1.0f / 60.0f;
		}
		agent.destination = player.transform.position;
	}

	public virtual void Attack()
	{
		if (timeToNextAttack <= 0.0f)
		{
			// Do the attacky thing
			//Debug.Log("Akashita Attack!");
			timeToNextAttack = timeBetweenAttacks;
		}
	}
}