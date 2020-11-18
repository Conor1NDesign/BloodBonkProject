using UnityEngine;

public class AkashitaAI : EnemyAI
{
	[Header("Projectile Settings")]
#pragma warning disable 0649
	[SerializeField]GameObject projectilePrefab;
#pragma warning restore 0649
	[SerializeField]float projectileSpeed = 1.0f;

	void FixedUpdate()
	{
		if (ragdolling)
			currentRagdollTime -= 1.0f / 60.0f;
		if (timeToNextAttack > 0.0f)
			timeToNextAttack -= 1.0f / 60.0f;
		if (currentStaggerTime > 0.0f)
			currentStaggerTime -= 1.0f / 60.0f;
		//if (timeToNextAttack > timeBetweenAttacks - attackLength)
			// TODO: Some particles or something, an indication of an attack
		if (timeToNextAttack < timeBetweenAttacks - attackLength &&
			currentStaggerTime <= 0.0f &&
			!ragdolling)
		{
			if (!agent.enabled)
			{
				// Do the attacky thing
				AkashitaProjectile projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation).GetComponent<AkashitaProjectile>();
				projectile.velocity = transform.forward * projectileSpeed;
				enemyManager.akashitaProjectiles.Add(projectile);
			}
			agent.enabled = true;
		}

		if (agent.enabled)
			base.MovementUpdate();
	}

	public override void Attack()
	{
		if (timeToNextAttack <= 0.0f)
		{
			// Do the attacky thing
			timeToNextAttack = timeBetweenAttacks;
			agent.enabled = false;
			animator.SetTrigger("Akashita_Attacking");
		}
	}

	public override void Ragdoll()
	{
		canvasObject.SetActive(false);
		animator.enabled = false;
		agent.enabled = false;
		ragdolling = true;
	}

	public override void Unragdoll()
	{
		canvasObject.SetActive(true);
		animator.enabled = true;
		agent.enabled = true;
		ragdolling = false;
		currentRagdollTime = ragdollTime;
	}
}