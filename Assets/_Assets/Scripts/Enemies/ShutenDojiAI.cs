using UnityEngine;

public class ShutenDojiAI : EnemyAI
{
#pragma warning disable 0649
	// temp
	[SerializeField]GameObject weapon;
#pragma warning restore 0649
	
	void FixedUpdate()
	{
		timeToNextAttack -= 1.0f / 60.0f;
		if (timeToNextAttack > timeBetweenAttacks - 0.4f)
			weapon.transform.Rotate(new Vector3(0.0f, 1f / attackLength, 0.0f));
		if (timeToNextAttack > timeBetweenAttacks - attackLength && timeToNextAttack < timeBetweenAttacks - 0.4f)
			weapon.transform.Rotate(new Vector3(0.0f, -4.0f / attackLength, 0.0f));
		if (timeToNextAttack < timeBetweenAttacks - attackLength)
		{
			if (!agent.enabled)
			{
				weapon.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
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
			animator.Play("Base Layer.Shuten_Attack");
		}
	}
}
