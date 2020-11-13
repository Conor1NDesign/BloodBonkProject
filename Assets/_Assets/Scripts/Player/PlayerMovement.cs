using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float increaseSpeed = 0.1f;

    [Header("Dash Meter Settings")]
    public float reduceDashMeter = 1f;
    public float fillDashMeter = 1f;
    public float maxDashMeter = 100f;
    public float refillTimer = 1f;
    private float currentDashMeter;

    // Camera vars
    Vector3 camF;
    Vector3 camR;
    Vector3 input;

    Transform mainCam;

    bool isDashing;

    [HideInInspector] public bool isAttacking;

    float actualSpeed; // Sprint or normal movement
    float maxTime; // Time to start refilling dash

    // Classes
    DashMeter dashMeter;

    // Component
    [HideInInspector] public Animator animator;

    [Header("Debug (NO TOUCH)")]
    public Game gameManager;
    public Weapon weapon;

    void Start()
    {
        gameManager = FindObjectOfType<Game>();
        weapon = FindObjectOfType<Weapon>();

        mainCam = FindObjectOfType<CameraFollow>().transform;
        dashMeter = FindObjectOfType<DashMeter>();
        animator = GetComponentInChildren<Animator>();

        currentDashMeter = maxDashMeter;
    }

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        Movement();
        //LookDirection();
    }

    public void HitDetection()
    {
        weapon.HitDetection();
    }

    private void PlayerInput()
    {
        if (!weapon.isSwinging)
        {
            // Get Player Input
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        }
        else if (weapon.isSwinging && !isAttacking)
        {
            input = new Vector3();
            animator.speed = weapon.attackSpeed;
            animator.SetTrigger("Attacking");

            isAttacking = true;
        }

        // Player Idle/Run Animation
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Running", true); // Running Animation
        }
        else
        {
            animator.SetBool("Running", false); // Idle Animation
        }

        // Player moves in camera view direction
        input = Vector3.ClampMagnitude(input, 1);

        camF = mainCam.forward;
        camR = mainCam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        Dashing();
        DashMeter();
    }


    private void Movement()
    {
        // Apply player movement
        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle + WrapAngle(mainCam.eulerAngles.y), 0f);

            transform.position += (camF * input.z + camR * input.x) * actualSpeed * Time.fixedDeltaTime;
        }
    }

    bool PlayerHasDash()
    {
        if (currentDashMeter >= reduceDashMeter)
        {
            return true;
        }
        return false;
    }
    
    // Detect if player presses dash
    private void Dashing()
    {
        if (Input.GetKey(KeyCode.Space) && PlayerHasDash() && !weapon.isSwinging ||
            Input.GetKey(KeyCode.LeftShift) && PlayerHasDash() && !weapon.isSwinging)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (actualSpeed >= dashSpeed)
                {
                    actualSpeed = dashSpeed;

                    isDashing = true;
                }
                else
                {
                    actualSpeed += increaseSpeed;
                }
            }
        }
        else
        {
            actualSpeed = moveSpeed;
            isDashing = false;
        }
    }

    private void DashMeter()
    {
        if (isDashing)
        {
            // Decreases Dash Meter
            currentDashMeter -= reduceDashMeter;
            maxTime = Time.time + refillTimer;
        }
        else
        {
            // Refills Dash Meter
            if (currentDashMeter < maxDashMeter)
            {
                if (maxTime <= Time.time)
                {
                    currentDashMeter += fillDashMeter;
                }
            }
        }

        // Set Dash UI
        dashMeter.SetDashMeter(currentDashMeter);
        dashMeter.SetMaxDashMeter(maxDashMeter);
    }

    private void LookDirection()
    {
        if (!weapon.isSwinging)
        {
            // Player Postion to Screen
            Vector2 posOnScreen = Camera.main.WorldToScreenPoint(transform.position);
            // Get Angle
            float angle = Mathf.Atan2(Input.mousePosition.x - posOnScreen.x, Input.mousePosition.y - posOnScreen.y) * Mathf.Rad2Deg;
            // Apply rotation
            transform.rotation = Quaternion.AngleAxis(angle + WrapAngle(mainCam.eulerAngles.y), Vector3.up);
        }
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
