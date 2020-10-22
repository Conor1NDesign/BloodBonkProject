using UnityEngine;

public class ShutenDojiAI : EnemyAI
{
#pragma warning disable 0649
	// temp
	[SerializeField]GameObject weapon;
#pragma warning restore 0649
	void FixedUpdate()
	{
		if (timeToNextAttack > 0.0f)
		{
			timeToNextAttack -= 1.0f / 60.0f;
			weapon.transform.Rotate(new Vector3(5.0f, 0.0f, 0.0f));
		}

		base.MovementUpdate();
	}

	public override void Attack()
	{
		if (timeToNextAttack <= 0.0f)
		{
			// Do the attacky thing
			//Debug.Log("Attack!");
			timeToNextAttack = timeBetweenAttacks;
			weapon.transform.Rotate(-150.0f, 0.0f, 0.0f);
		}
	}
}
