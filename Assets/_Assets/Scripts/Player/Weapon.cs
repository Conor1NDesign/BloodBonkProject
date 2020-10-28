using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private List<List<Vector3>> previousPosition;

    [HideInInspector]
    public bool isSwinging;

    // Debug
    private bool isAttacking;

    // Hit Detection
    public float range = 3f;
    public LayerMask enemyMask;
    public float hitDetectionRange = 40f;
    Vector2 mouseInput;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        DisableWeapon();
    }

    // Plays attacking animation
    void Attack()
    {
        mouseInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        isSwinging = true;
        transform.GetComponent<Renderer>().enabled = true;
        animator.Play("Attack");
    }

    // Enable weapon collider
    public void EnableWeapon()
    {
        previousPosition = new List<List<Vector3>>();
        HitDetection();

        // COLLIDER
        //transform.GetComponent<BoxCollider>().enabled = true;

        // DEBUG LINES
        //isAttacking = true;
    }

    // Disable weapon collider
    public void DisableWeapon()
    {
        transform.GetComponent<Renderer>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        isSwinging = false;
        isAttacking = false;
    }

    public void HitDetection()
    {
        Vector3 playerPos = transform.parent.position;

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

        // Loop through all enemies
        foreach (Collider e in enemiesInRange)
        {
            // DEBUGGING
            //float angle = Vector3.Angle(transform.parent.forward, e.gameObject.transform.position - playerPos);

            // Hit Detection cone facing enemy
            if (Vector3.Angle(transform.parent.forward, e.gameObject.transform.position - playerPos) < hitDetectionRange)
            {
                WeaponDetect detect = e.gameObject.GetComponent<WeaponDetect>();
                detect.TakeDamage();
            }
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
