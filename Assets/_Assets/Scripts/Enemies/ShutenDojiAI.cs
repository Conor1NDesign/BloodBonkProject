using UnityEngine;
using UnityEngine.AI;

public class ShutenDojiAI : EnemyAI
{
	Rigidbody[] ragdollRbs;
	Collider[] ragdollColliders;

	[Header("Attack Settings")]
#pragma warning disable 0649
	[SerializeField]GameObject bottle;
#pragma warning restore 0649
	Vector3 bottlePosInitial;
	bool dealtDamage = true;

	void Awake()
	{
		// Cache this GameObject's navmesh agent
		agent = GetComponent<NavMeshAgent>();
		healthBar = GetComponentInChildren<HealthBar>();
		animator = GetComponentInChildren<Animator>();
		health = currentMaxHealth;
		canvasObject = GetComponentInChildren<FollowCamera>().gameObject;
		ragdollRbs = GetComponentsInChildren<Rigidbody>();
		ragdollColliders = GetComponentsInChildren<Collider>();
		for (int i = 0; i < ragdollRbs.Length; i++)
		{
			ragdollRbs[i].isKinematic = true;
			ragdollColliders[i].enabled = false;
		}
		
	}

	void FixedUpdate()
	{
		if (ragdolling)
			ragdollTime -= 1.0f / 60.0f;
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

	public override void Ragdoll()
	{
		canvasObject.SetActive(false);
		animator.enabled = false;
		agent.enabled = false;
		ragdollColliders[ragdollColliders.Length - 1].enabled = false;
		for (int i = 0; i < ragdollRbs.Length; i++)
		{
			ragdollRbs[i].isKinematic = false;
			ragdollColliders[i].enabled = true;
		}
		ragdolling = true;
	}

	public override void Unragdoll()
	{
		canvasObject.SetActive(true);
		animator.enabled = true;
		agent.enabled = true;
		for (int i = 0; i < ragdollRbs.Length; i++)
		{
			ragdollRbs[i].isKinematic = true;
			ragdollColliders[i].enabled = false;
		}
		ragdollColliders[ragdollColliders.Length - 1].enabled = true;
		ragdolling = false;
	}
}
