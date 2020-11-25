using UnityEngine;

public class AkashitaProjectile : MonoBehaviour
{
	[HideInInspector]public Vector3 velocity = Vector3.zero;
	public float timeLeft = 3.0f;
#pragma warning disable 0649
	AudioSource audioSource;
#pragma warning restore 0649
	public AudioClip hitSound;
	[HideInInspector]public float damage = 10.0f;

	public AudioSource projectileHitSound;

	void Update()
	{
		timeLeft -= Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Weapon")))
		{
			audioSource.PlayOneShot(hitSound);
			Destroy(gameObject);
		}
	}
}