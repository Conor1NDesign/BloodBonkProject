using UnityEngine;

public class ShutenDojiAI : MonoBehaviour, IAttacker
{
	// Designers! Feel free to change these two!
	public float range { get; } = 1.4f;
	public float timeBetweenAttacks { get; set; } = 0.5f;
	// These two are used internally
	public float currentDistanceToPlayer { get; private set; } = 10.0f;
	public float timeToNextAttack { get; private set; } = 0.0f;

#pragma warning disable 0649
	// temp
	[SerializeField]GameObject weapon;
	// not temp
	[SerializeField]GameObject player;
#pragma warning restore 0649
	// The movement speed of the Shuten Doji
	[SerializeField]float movementSpeed = 1.0f;
	// The speed at which the Shuten Doji turns to face the player
	[SerializeField]float rotationSpeed = 1.0f;
	// The distance from the player that the Shuten Doji will approach to
	[SerializeField]float distanceFromPlayer = 1.0f;
	// The amount of variation allowed in the rotation towards the player
	[SerializeField]float allowedRotationVariation = 0.1f;
	// The amount of variation allowed in the distance from the player
	[SerializeField]float allowedDistanceVariation = 0.1f;
	
	// The rigidbody of the Shuten Doji
	Rigidbody shutenDojiRigidbody;
	
	
	void Awake()
	{
		// Cache this GameObject's rigidbody
		shutenDojiRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if (timeToNextAttack > 0.0f)
		{
			timeToNextAttack -= 1.0f / 60.0f;
			weapon.transform.Rotate(new Vector3(5.0f, 0.0f, 0.0f));
		}

		// Find the direction to the player
		Vector3 toPlayer = player.transform.position - transform.position;
		toPlayer.y = 0.0f;
		currentDistanceToPlayer = toPlayer.magnitude;
		
		// If the enemy isn't within range of the player, move to range
		Vector3 toDistanceFromPlayer = toPlayer - toPlayer.normalized * distanceFromPlayer;
		if (toDistanceFromPlayer.sqrMagnitude > allowedDistanceVariation)
			shutenDojiRigidbody.velocity = toDistanceFromPlayer.normalized * movementSpeed;
		else
			shutenDojiRigidbody.velocity = Vector3.zero;

		// Rotate towards the player
		float directionToPlayer = Vector3.Dot(Vector3.Cross(transform.forward, toPlayer), transform.up);
		
		directionToPlayer = directionToPlayer > allowedRotationVariation ? 1.0f :
			directionToPlayer < -allowedRotationVariation ? -1.0f : 0.0f;

		shutenDojiRigidbody.angularVelocity = new Vector3(0, directionToPlayer * rotationSpeed, 0);
	}

	public void Attack()
	{
		if (timeToNextAttack <= 0.0f)
		{
			// Do the attacky thing
			//Debug.Log("Attack!");
			timeToNextAttack = timeBetweenAttacks;
			weapon.transform.Rotate(-150.0f, 0.0f, 0.0f);
		}
	}
}
