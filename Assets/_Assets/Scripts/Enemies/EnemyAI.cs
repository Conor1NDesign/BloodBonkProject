using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	[HideInInspector]public float currentDistanceToPlayer = 10.0f;
	[HideInInspector]public EnemyManager enemyManager;
	public GameObject player;
	
	public float range = 1.0f;
	[SerializeField]protected float timeBetweenAttacks = 1.0f;
	// The distance from the player that the enemy will approach to
	[SerializeField]protected float distanceFromPlayer = 1.0f;
	//HEALTH!
	public float maxHealth = 100.0f;
	[HideInInspector]public float health = 100.0f;
	public HealthBar healthBar;
	public float damage = 10.0f;
	
	// The rigidbody of the enemy
	protected NavMeshAgent agent;
	protected float timeToNextAttack = 0.0f;

	public void TakeDamage(float damage)
	{
		health -= damage;
		healthBar.SetHealth(health);
	}

	public float GetHealth()
	{
		return health;
	}
	
	void Awake()
	{
		// Cache this GameObject's navmesh agent
		agent = GetComponent<NavMeshAgent>();
		healthBar = GetComponentInChildren<HealthBar>();
		healthBar.SetMaxHealth(maxHealth);
		healthBar.SetHealth(health);
		health = maxHealth;
	}

	void FixedUpdate()
	{
		if (timeToNextAttack > 0.0f)
		{
			timeToNextAttack -= 1.0f / 60.0f;
		}

		MovementUpdate();
	}

	protected void MovementUpdate()
	{
		Vector3 toPlayer = player.transform.position - transform.position;
		agent.destination = player.transform.position - ((toPlayer).normalized * distanceFromPlayer);
		currentDistanceToPlayer = toPlayer.magnitude;
		if (currentDistanceToPlayer < range)
		{
			// Rotate towards the player
			float directionToPlayer = Vector3.Dot(Vector3.Cross(transform.forward, toPlayer), transform.up);
			
			directionToPlayer = directionToPlayer > 1.0f ? 0.05f :
				directionToPlayer < -1.0f ? -0.05f : 0.0f;
			
			transform.Rotate(new Vector3(0, directionToPlayer * agent.angularSpeed, 0));

			// I'll leave this idiocy here in comment form.
			// Why didn't I just use transform.rotate from the start?
			// No idea!
			/*float newYRotation = transform.rotation.y + directionToPlayer;
			if (newYRotation > 1.0f)
				newYRotation -= 2.0f;
			if (newYRotation < -1.0f)
				newYRotation += 2.0f;
			transform.rotation = new Quaternion(0,
				transform.rotation.y + directionToPlayer,
				0, transform.rotation.w);*/
		}
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