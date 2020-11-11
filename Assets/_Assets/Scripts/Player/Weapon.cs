using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float attackSpeed = 1f;

    [HideInInspector]
    public bool isSwinging;

    // Debug
    private bool isAttacking;

    // Hit Detection
    public float range = 3f;
    public LayerMask enemyMask;
    [Tooltip("How wide the angle/cone the attack will be")] public float hitDetectionRange = 70f;
    Vector2 mouseInput;

    CameraShake camShake;
    PlayerStats stats;

    private void Awake()
    {
        camShake = FindObjectOfType<CameraShake>();
        stats = FindObjectOfType<PlayerStats>();
        SetAttackSpeed(attackSpeed);
        DisableWeapon();
    }

    public void SetAttackSpeed(float attackSpeed)
    {
        //animator.speed = attackSpeed;
    }

    // Plays attacking animation
    void Attack()
    {
        mouseInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        isSwinging = true;
        //transform.GetComponent<Renderer>().enabled = true; // Show Weapon
    }

    // Enable weapon collider
    public void EnableWeapon()
    {
        HitDetection();

        // COLLIDER
        //transform.GetComponent<BoxCollider>().enabled = true;

        // DEBUG LINES
        //isAttacking = true;
    }

    // Disable weapon collider
    public void DisableWeapon()
    {
        //transform.GetComponent<Renderer>().enabled = false;
        isSwinging = false;
        isAttacking = false;
    }

    public void HitDetection()
    {
        Vector3 playerPos = transform.root.position;

        // DEBUGGING
        // =====================================================================================================
        //float playerAngle = transform.parent.eulerAngles.y * Mathf.Deg2Rad;

        //// Player Angle
        //Vector3 endPos = new Vector3(playerPos.x + (range * Mathf.Sin(playerAngle)), playerPos.y, playerPos.z + (range * Mathf.Cos(playerAngle)));
        //Debug.DrawLine(playerPos, endPos, Color.yellow);

        //// Positive
        //float posAngle = playerAngle + posRange;
        //endPos = new Vector3(playerPos.x + (range * Mathf.Sin(posAngle)), playerPos.y, playerPos.z + (range * Mathf.Cos(posAngle)));
        //Debug.DrawLine(playerPos, endPos, Color.green);

        //// Negative
        //float negAngle = playerAngle - negRange;
        //endPos = new Vector3(playerPos.x + (range * Mathf.Sin(negAngle)), playerPos.y, playerPos.z + (range * Mathf.Cos(negAngle)));
        //Debug.DrawLine(playerPos, endPos, Color.red);
        // =====================================================================================================

        // All enemies within range
        Collider[] enemiesInRange = Physics.OverlapSphere(playerPos, range, enemyMask);

        bool enemyHit = false;

        // Loop through all enemies
        foreach (Collider e in enemiesInRange)
        {
            // DEBUGGING
            //float angle = Vector3.Angle(transform.parent.forward, e.gameObject.transform.position - playerPos);

            // Hit Detection cone if facing enemy
            if (Vector3.Angle(transform.parent.forward, e.gameObject.transform.position - playerPos) < hitDetectionRange)
            {
                WeaponDetect detect = e.gameObject.GetComponentInParent<WeaponDetect>();
                detect.TakeDamage(damage);
                stats.Lifesteal(damage);

                enemyHit = true;
            }
        }

        if (enemyHit)
        {
            StartCoroutine(camShake.Shake());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging)
        {
            Attack();
        }

        // Debugging
        if (isAttacking)
        {
           HitDetection();
        }
    }
}
