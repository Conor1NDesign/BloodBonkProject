using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float increaseSpeed = 0.1f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothTimeAttack = 0.1f;
    float turnSmoothVelocity;

    [Header("Dash Meter Settings")]
    public float reduceDashMeter = 1f;
    public float fillDashMeter = 1f;
    public float maxDashMeter = 100f;
    public float refillTimer = 1f;
    private float currentDashMeter;
    TrailRenderer dashParticle;

    [Header("Player Sounds")]
    public AudioSource deathSound;
    public AudioSource dashSound;
	public AudioSource akashitaProjectileHitSound;

    bool playDashSound;

    [HideInInspector] public Vector2 posOnScreen; // Apply on attack
    [HideInInspector] public Vector2 mouseInput;
    [HideInInspector] public bool hasLunged;
    [HideInInspector] public bool isAttacking;

    [HideInInspector] public Vector3 input; // Player Movement

    Transform mainCam;
    Transform attackDir;

    bool isDashing;
    
    float actualSpeed; // Sprint or normal movement
    float maxTime; // Time to start refilling dash

    // Classes
    DashMeter dashMeter;

    // Component
    [HideInInspector] public Animator animator;

    Game gameManager;
    Weapon weapon;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        attackDir = transform.GetChild(2).transform;
        dashParticle = GetComponentInChildren<TrailRenderer>();
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
        if (!weapon.isSwinging)
        {
            if (!gameManager.isDead)
            {
                // Get Player Input
                input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            }
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

        Dashing();
        DashMeter();
    }


    private void Movement()
    {
        // Apply player movement
        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir * actualSpeed * Time.fixedDeltaTime;
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
                dashParticle.enabled = true;

                if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift))
                {
                    playDashSound = true;
                }

                if (actualSpeed >= dashSpeed / 1.5f)
                {
                    if (dashSound != null && playDashSound)
                    {
                        dashSound.Play();
                        playDashSound = false;
                    }
                }

                // Check if dash speed is reached
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
            // No longer dashing
            actualSpeed = moveSpeed;
            isDashing = false;
            dashParticle.enabled = false;
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

    // If player attacked
    private void LookDirection()
    {
        if (weapon.isSwinging)
        {
            // Get Angle
            float targetAngle = Mathf.Atan2(mouseInput.x - posOnScreen.x, mouseInput.y - posOnScreen.y) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTimeAttack);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if (!hasLunged)
            {
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                transform.position += moveDir * weapon.lungeDistance * Time.fixedDeltaTime;
            }
        }

        Vector2 atkDirInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePos = Camera.main.WorldToScreenPoint(transform.position);

        // Get Angle
        float atkDirAngle = Mathf.Atan2(atkDirInput.x - mousePos.x, atkDirInput.y - mousePos.y) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
        // Apply rotation
        Vector3 atkDir = Quaternion.Euler(0f, atkDirAngle, 0f) * Vector3.up;
        // Attacking Direction
        attackDir.rotation = Quaternion.AngleAxis(atkDirAngle + 180f, atkDir);
    }
}
