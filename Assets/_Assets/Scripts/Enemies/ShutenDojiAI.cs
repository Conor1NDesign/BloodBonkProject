using UnityEngine;

public class ShutenDojiAI : EnemyAI
{
	[Header("Attack Settings")]
#pragma warning disable 0649
	[SerializeField]GameObject bottle;
#pragma warning restore 0649
	Vector3 bottlePosInitial;
	bool dealtDamage = true;
	
	void FixedUpdate()
	{
		if (timeToNextAttack > 0.0f)
			timeToNextAttack -= 1.0f / 60.0f;
		if (currentStaggerTime > 0.0f)
			currentStaggerTime -= 1.0f / 60.0f;
		if (timeToNextAttack > timeBetweenAttacks - attackLength && !dealtDamage)
		{
			if (Physics.Linecast(bottlePosInitial, bottle.transform.position, LayerMask.GetMask("Player")))
			{
				player.GetComponent<PlayerStats>().TakeDamage(damage);
				dealtDamage = true;
			}
			else
				bottlePosInitial = bottle.transform.position;
		}
		if (timeToNextAttack < timeBetweenAttacks - attackLength && currentStaggerTime <= 0.0f)
		{
			if (!agent.enabled)
			{
				dealtDamage = true;
				agent.enabled = true;
			}
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
			animator.SetTrigger("Shuten_Attacking");
			bottlePosInitial = bottle.transform.position;
		}
	}
}
