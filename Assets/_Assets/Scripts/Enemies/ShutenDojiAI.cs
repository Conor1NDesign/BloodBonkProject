using UnityEngine;

public class ShutenDojiAI : EnemyAI
{
	bool dealtDamage = true;
	
	void FixedUpdate()
	{
		timeToNextAttack -= 1.0f / 60.0f;
		// I don't even know anymore. WHY SEOUIFHUDSJOSDCOIJSEDOJISVEOJIOIJKSEF
		if (timeToNextAttack < timeBetweenAttacks - (attackLength / 2) &&
			timeToNextAttack > timeBetweenAttacks - (3 * attackLength / 5) &&
			!dealtDamage)
		{
			Collider[] playersInRange = Physics.OverlapSphere(transform.root.position, (3 * range / 5), LayerMask.GetMask("Player"));
			for (int i = 0; i < playersInRange.Length; i++)
			{
				playersInRange[i].gameObject.GetComponentInParent<PlayerStats>().TakeDamage(damage);
				dealtDamage = true;
			}
		}
		if (timeToNextAttack < timeBetweenAttacks - attackLength)
		{
			if (!agent.enabled)
			{
				dealtDamage = true;
				animator.Play("Base Layer.Shuten_WalkCycle");
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
			dealtDamage = false;
			animator.Play("Base Layer.Shuten_Attack");
		}
	}
}
