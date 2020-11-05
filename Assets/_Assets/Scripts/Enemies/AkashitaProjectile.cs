using UnityEngine;

public class AkashitaProjectile : MonoBehaviour
{
	[HideInInspector]public Vector3 velocity = Vector3.zero;
	public float timeLeft = 3.0f;

	void Update()
	{
		timeLeft -= Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")))
			Destroy(gameObject);
	}
}