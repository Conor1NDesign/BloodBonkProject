using UnityEngine;

public class ShutenDojiAI : MonoBehaviour
{
#pragma warning disable 0649
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
		// Find the direction to the player
		Vector3 toPlayer = player.transform.position - transform.position;
		toPlayer.y = 0.0f;
		
		// If the enemy isn't within range of the player, move to range
		Vector3 toDistanceFromPlayer = toPlayer - toPlayer.normalized * distanceFromPlayer;
		if (toDistanceFromPlayer.sqrMagnitude > allowedDistanceVariation)
			shutenDojiRigidbody.velocity = toDistanceFromPlayer.normalized * movementSpeed;

		// Rotate towards the player
		float directionToPlayer = Vector3.Dot(Vector3.Cross(transform.forward, toPlayer), transform.up);
		
		directionToPlayer = directionToPlayer > allowedRotationVariation ? 1.0f :
			directionToPlayer < -allowedRotationVariation ? -1.0f : 0.0f;

		shutenDojiRigidbody.angularVelocity = new Vector3(0, directionToPlayer * rotationSpeed, 0);
	}
}
