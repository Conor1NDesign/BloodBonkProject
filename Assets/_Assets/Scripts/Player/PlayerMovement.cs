﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform mainCam;
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;

    float actualSpeed;

    void FixedUpdate()
    {
        Movement();
        LookDirection();
    }

    private void Movement()
    {
        // Get Player Input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        // Player moves in camera view direction
        input = Vector2.ClampMagnitude(input, 1);

        Vector3 camF = mainCam.forward;
        Vector3 camR = mainCam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        Dashing();

        transform.position += (camF * input.y + camR * input.x) * actualSpeed * Time.fixedDeltaTime;
    }

    private void Dashing()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {
            actualSpeed = dashSpeed;
        }
        else
        {
            actualSpeed = moveSpeed;
        }
    }

    private void LookDirection()
    {
        // Player Postion to Screen
        Vector2 posOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        // Get Angle
        float angle = Mathf.Atan2(Input.mousePosition.x - posOnScreen.x, Input.mousePosition.y - posOnScreen.y) * Mathf.Rad2Deg;
        // Apply rotation
        transform.rotation = Quaternion.AngleAxis(angle + WrapAngle(mainCam.eulerAngles.y), Vector3.up);
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    private static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }
}