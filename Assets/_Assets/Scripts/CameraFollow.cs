using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        //Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, 0.5f);
        transform.position = desiredPos;

        transform.LookAt(target);

    }
}
