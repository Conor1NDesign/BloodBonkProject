using UnityEngine;

public class AkashitaAI : MonoBehaviour, IAttacker
{
	public float currentDistanceToPlayer { get; private set; } = 10.0f;
	public float range { get; } = 4.3f;
	public float attackTime { get; private set; } = 0.0f;

#pragma warning disable 0649
	[SerializeField]GameObject player;
#pragma warning restore 0649
	// The movement speed of the Akashita
	[SerializeField]float movementSpeed = 1.0f;
	// The speed at which the Akashita turns to face the player
	[SerializeField]float rotationSpeed = 1.0f;
	// The distance from the player that the Akashita will approach to
	[SerializeField]float distanceFromPlayer = 4.0f;
	// The amount of variation allowed in the rotation towards the player
	[SerializeField]float allowedRotationVariation = 0.1f;
	// The amount of variation allowed in the distance from the player
	[SerializeField]float allowedDistanceVariation = 0.1f;
	
	// The rigidbody of the Akashita
	Rigidbody akashitaRigidbody;
	
	
	void Awake()
	{
		// Cache this GameObject's rigidbody
		akashitaRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if (attackTime > 0.0f)
		{
			attackTime -= 1.0f / 60.0f;
		}

		// Find the direction to the player
		Vector3 toPlayer = player.transform.position - transform.position;
		toPlayer.y = 0.0f;
		currentDistanceToPlayer = toPlayer.magnitude;
		Debug.Log(currentDistanceToPlayer);
		
		// If the enemy isn't within range of the player, move to range
		Vector3 toDistanceFromPlayer = toPlayer - toPlayer.normalized * distanceFromPlayer;
		if (toDistanceFromPlayer.sqrMagnitude > allowedDistanceVariation)
			akashitaRigidbody.velocity = toDistanceFromPlayer.normalized * movementSpeed;

		// Rotate towards the player
		float directionToPlayer = Vector3.Dot(Vector3.Cross(transform.forward, toPlayer), transform.up);
		
		directionToPlayer = directionToPlayer > allowedRotationVariation ? 1.0f :
			directionToPlayer < -allowedRotationVariation ? -1.0f : 0.0f;

		akashitaRigidbody.angularVelocity = new Vector3(0, directionToPlayer * rotationSpeed, 0);
	}

	public void Attack()
	{
		if (attackTime <= 0.0f)
		{
			// Do the attacky thing
			Debug.Log("Akashita Attack!");
			attackTime = 1.0f;
		}
	}
}
