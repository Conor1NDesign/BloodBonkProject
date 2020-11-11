using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;

    [Header("Dash Meter Settings")]
    public float reduceDashMeter = 1f;
    public float fillDashMeter = 1f;
    public float maxDashMeter = 100f;
    public float refillTimer = 1f;
    private float currentDashMeter;

    // Camera vars
    Vector3 camF;
    Vector3 camR;
    Vector2 input;

    Transform mainCam;

    bool isDashing;

    float actualSpeed; // Sprint or normal movement
    float maxTime; // Time to start refilling dash

    // Classes
    DashMeter dashMeter;
    public GameObject weaponHolder;
    public SpawnPower power;

    [Header("Debug")]
    public Game gameManager;
    public Weapon weapon;

    bool setup = true;

    void Start()
    {
        gameManager = FindObjectOfType<Game>();
        weapon = FindObjectOfType<Weapon>();

        mainCam = FindObjectOfType<CameraFollow>().transform;
        dashMeter = FindObjectOfType<DashMeter>();
        currentDashMeter = maxDashMeter;
    }

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        Movement();
        LookDirection();
    }

    private void PlayerInput()
    {
        // Get Player Input
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //if (!isDashing)
        //{
        //    input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //}

        // Player moves in camera view direction
        input = Vector2.ClampMagnitude(input, 1);

        camF = mainCam.forward;
        camR = mainCam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        Dashing();
        DashMeter();

        //actualSpeed = Mathf.Lerp(0, actualSpeed, 0.9f * Time.deltaTime);
    }


    private void Movement()
    {
        // Apply player movement
        transform.position += (camF * input.y + camR * input.x) * actualSpeed * Time.fixedDeltaTime;
    }

    
    private void Dashing()
    {
        actualSpeed = moveSpeed;
        isDashing = false;

        // Player not swinging weapon
        if (!weapon.isSwinging)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    // Check if player has enough dash
                    if (currentDashMeter >= reduceDashMeter)
                    {
                        actualSpeed = dashSpeed;
                        isDashing = true;
                    }
                }
            }
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
