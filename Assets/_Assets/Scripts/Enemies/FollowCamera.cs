using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public Camera target;
	Quaternion baseRotation;
	
	void Awake()
	{
		target = Camera.main;
		baseRotation = transform.rotation;
	}

    void Update()
	{
		transform.LookAt(transform.position - target.transform.rotation * Vector3.forward, target.transform.rotation * Vector3.up);
	}
}
