using UnityEngine;

public class AkashitaAI : EnemyAI
{
#pragma warning disable 0649
	[SerializeField]GameObject projectilePrefab;
#pragma warning restore 0649
	[SerializeField]float projectileSpeed = 1.0f;

	public override void Attack()
	{
		if (timeToNextAttack <= 0.0f)
		{
			// Do the attacky thing
			timeToNextAttack = timeBetweenAttacks;

			AkashitaProjectile projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation).GetComponent<AkashitaProjectile>();
			projectile.velocity = transform.forward * projectileSpeed;
			enemyManager.akashitaProjectiles.Add(projectile);
		}
	}
}