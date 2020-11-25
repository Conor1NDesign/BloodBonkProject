using UnityEngine;
using UnityEngine.AI;

public class ShutenDojiAI : EnemyAI
{
	Rigidbody[] ragdollRbs;
	Vector3[] ragdollPositions;

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
		renderers = gameObject.GetComponentsInChildren<Renderer>();
		ragdollRbs = GetComponentsInChildren<Rigidbody>();
		ragdollPositions = new Vector3[ragdollRbs.Length];
		for (int i = 0; i < ragdollRbs.Length; i++)
		{
			ragdollRbs[i].isKinematic = true;
			ragdollPositions[i] = ragdollRbs[i].transform.position;
		}
		
	}

	void FixedUpdate()
	{
		if (ragdolling)
			currentRagdollTime -= 1.0f / 60.0f;
		if (timeToNextAttack > 0.0f)
			timeToNextAttack -= 1.0f / 60.0f;
		if (currentStaggerTime > 0.0f)
			currentStaggerTime -= 1.0f / 60.0f;
		if (timeToNextAttack > timeBetweenAttacks - attackLength &&
			!dealtDamage &&
			!ragdolling)
		{
			if (Physics.Linecast(bottlePosInitial, bottle.transform.position, LayerMask.GetMask("Player")))
			{
				player.GetComponent<PlayerStats>().TakeDamage(damage);
				dealtDamage = true;
			}
			else
				bottlePosInitial = bottle.transform.position;
		}
		if (timeToNextAttack < timeBetweenAttacks - attackLength &&
			currentStaggerTime <= 0.0f &&
			!ragdolling)
		{
			if (!agent.enabled)
			{
				dealtDamage = true;
				agent.enabled = true;
			}
		}
		if (currentFlashTime > 0.0f)
			currentFlashTime -= 1.0f / 60.0f;
		else
			for (int i = 0; i < renderers.Length; i++)
				renderers[i].material.SetFloat("Vector1_9C3EE106", 1);
		
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
			if (attackSound != null)
				audioSource.PlayOneShot(attackSound);
			animator.SetTrigger("Shuten_Attacking");
			bottlePosInitial = bottle.transform.position;
		}
	}

	public override void Ragdoll()
	{
		canvasObject.SetActive(false);
		animator.enabled = false;
		agent.enabled = false;
		for (int i = 0; i < ragdollRbs.Length; i++)
		{
			ragdollRbs[i].isKinematic = false;
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
			ragdollRbs[i].position = ragdollPositions[i];
		}
		ragdolling = false;
		currentRagdollTime = ragdollTime;
	}
}
