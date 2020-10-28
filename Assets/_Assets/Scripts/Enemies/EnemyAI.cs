using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	[HideInInspector]public float currentDistanceToPlayer = 10.0f;
	public GameObject player;
	
	public float range = 1.0f;
	[SerializeField]protected float timeBetweenAttacks = 1.0f;
	// The movement speed of the enemy
	[SerializeField]float movementSpeed = 1.0f;
	// The speed at which the enemy turns to face the player
	[SerializeField]float rotationSpeed = 1.0f;
	// The distance from the player that the enemy will approach to
	[SerializeField]float distanceFromPlayer = 1.0f;
	// The amount of variation allowed in the rotation towards the player
	[SerializeField]float allowedRotationVariation = 0.1f;
	// The amount of variation allowed in the distance from the player
	[SerializeField]float allowedDistanceVariation = 0.1f;
	//HEALTH!
	[SerializeField]float maxHealth = 100.0f;
	float health = 100.0f;
	
	// The rigidbody of the enemy
	Rigidbody enemyRigidBody;
	protected float timeToNextAttack = 0.0f;
	
	public void Damage(float damage)
	{
		health -= damage;
	}

	public float GetHealth()
	{
		return health;
	}
	
	void Awake()
	{
		// Cache this GameObject's rigidbody
		enemyRigidBody = GetComponent<Rigidbody>();
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

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Weapon"))
		{
			health -= 10.0f;
		}
	}

	protected void MovementUpdate()
	{
		// Find the direction to the player
		Vector3 toPlayer = player.transform.position - transform.position;
		toPlayer.y = 0.0f;
		currentDistanceToPlayer = toPlayer.magnitude;
		
		// If the enemy isn't within range of the player, move to range
		Vector3 toDistanceFromPlayer = toPlayer - toPlayer.normalized * distanceFromPlayer;
		if (toDistanceFromPlayer.sqrMagnitude > allowedDistanceVariation)
			enemyRigidBody.velocity = toDistanceFromPlayer.normalized * movementSpeed;
		else
			enemyRigidBody.velocity = Vector3.zero;

		// Rotate towards the player
		float directionToPlayer = Vector3.Dot(Vector3.Cross(transform.forward, toPlayer), transform.up);
		
		directionToPlayer = directionToPlayer > allowedRotationVariation ? 1.0f :
			directionToPlayer < -allowedRotationVariation ? -1.0f : 0.0f;

		enemyRigidBody.angularVelocity = new Vector3(0, directionToPlayer * rotationSpeed, 0);
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