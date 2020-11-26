using UnityEngine;

public class AkashitaProjectile : MonoBehaviour
{
	[HideInInspector]public Vector3 velocity = Vector3.zero;
	public float timeLeft = 3.0f;
	[HideInInspector]public float damage = 10.0f;

	void Update()
	{
		timeLeft -= Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		// If the projectile hits a wall or other world objects, remove it
		if (!(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Powerup")))
			Destroy(gameObject);
	}
}