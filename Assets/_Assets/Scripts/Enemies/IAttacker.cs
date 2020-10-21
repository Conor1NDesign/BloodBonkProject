public interface IAttacker
{
	float currentDistanceToPlayer { get; }
	float range { get; }
	float attackTime { get; }

	void Attack();
}
