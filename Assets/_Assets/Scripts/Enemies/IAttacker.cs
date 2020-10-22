﻿using UnityEngine;

public interface IAttacker
{
	float range { get; }
	float timeBetweenAttacks { get; set; }
	float currentDistanceToPlayer { get; }
	float timeToNextAttack { get; }

	void Attack();
}
