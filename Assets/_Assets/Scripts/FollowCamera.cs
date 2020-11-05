using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public Camera target;
	
	void Awake()
	{
		target = Camera.main;
	}

    void Update()
	{
		transform.LookAt(target.transform.position);
	}
}
