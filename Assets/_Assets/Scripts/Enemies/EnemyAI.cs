using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour
{
	[HideInInspector]public float currentDistanceToPlayer = 10.0f;
	[HideInInspector]public EnemyManager enemyManager;
	
	[Header("AI Settings")]
	public GameObject player;
	public float range = 1.0f;
	[SerializeField]protected float timeBetweenAttacks = 1.0f;
	[SerializeField]protected float attackLength = 1.0f;
	// The distance from the player that the enemy will approach to
	[SerializeField]protected float distanceFromPlayer = 1.0f;
	public float baseDamage = 10.0f;
	public float damage = 10.0f;
	[Tooltip("How long this enemy staggers when hit")]public float staggerTime = 1.0f;
	[SerializeField]protected float currentStaggerTime = 0.0f;
	//HEALTH!
	[Header("Health Settings")]
	public float baseMaxHealth = 100.0f;
	public float currentMaxHealth = 0.0f;
	[HideInInspector]public float health = 100.0f;
	public HealthBar healthBar;

	[Header("Ragdoll Settings")]
	public float ragdollTime = 1.0f;
	protected bool ragdolling = false;
#pragma warning disable 0649
	protected GameObject canvasObject;
#pragma warning restore 0649
	
	// Cached components of the enemy
	protected Animator animator;
	protected NavMeshAgent agent;
	protected float timeToNextAttack = 0.0f;

	public void TakeDamage(float damage)
	{
		health -= damage;
		healthBar.SetMaxHealth(currentMaxHealth);
		healthBar.SetHealth(health);
		currentStaggerTime = staggerTime;
		agent.enabled = false;
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
		animator = GetComponentInChildren<Animator>();
		health = currentMaxHealth;
		canvasObject = GetComponentInChildren<FollowCamera>().gameObject;
	}

	void FixedUpdate()
	{
		if (ragdolling)
			ragdollTime -= 1.0f / 60.0f;
		if (timeToNextAttack > 0.0f)
			timeToNextAttack -= 1.0f / 60.0f;
		if (currentStaggerTime > 0.0f)
			currentStaggerTime -= 1.0f / 60.0f;
		if (timeToNextAttack < timeBetweenAttacks - attackLength && currentStaggerTime <= 0.0f)
			agent.enabled = true;

		if (agent.enabled)
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
			timeToNextAttack = timeBetweenAttacks;
			agent.enabled = false;
		}
	}

	public abstract void Ragdoll();
	public abstract void Unragdoll();
}