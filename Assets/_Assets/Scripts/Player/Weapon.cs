using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private List<List<Vector3>> previousPosition;

    [HideInInspector]
    public bool isSwinging;

    bool isAttacking;

    // Raycast Stuff
    public bool raycastHit = false;
    public RaycastHit hit;
    public WeaponDetect detect;

    // Hit Detection
    public float range;
    public LayerMask enemyMask;
    Vector2 mouseInput;
    public float posRange;
    public float negRange;

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
        transform.GetComponent<BoxCollider>().enabled = true;
        isAttacking = true;
    }

    // Disable weapon collider
    public void DisableWeapon()
    {
        transform.GetComponent<Renderer>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        isSwinging = false;
        isAttacking = false;
    }

    // Positions for raycast
    void SetPreviousPosition()
    {
        List<Vector3> position = new List<Vector3>();

        // Loop through child objects
        foreach (Transform child in transform)
        {
            position.Add(child.position);
        }

        previousPosition.Add(position);
    }

    // Raycast
    private void RaycastDetect()
    {
        // Add current positions to list
        SetPreviousPosition();

        for (int l = 0; l < previousPosition.Count - 1; l++)
        {
            for (int i = 0; i < previousPosition[0].Count; i++)
            {
                ////float distance = Vector3.Distance(previousPosition[l][i], previousPosition[l + 1][i]);
                //if (Physics.Raycast(previousPosition[l][i], previousPosition[l + 1][i], out hit))
                //{
                //    if (hit.collider.CompareTag("Enemy"))
                //    {
                //        detect.TakeDamage();
                //    }
                //}

                // Debugging
                Debug.DrawLine(previousPosition[l][i], previousPosition[l + 1][i], Color.green);
            }
        }
    }

    public void HitDetection()
    {
        Vector3 playerPos = transform.parent.position;
        float playerAngle = transform.eulerAngles.y * Mathf.Deg2Rad;

        // Player Angle
        Vector3 endPos = new Vector3(playerPos.x + (3f * Mathf.Sin(playerAngle)), playerPos.y, playerPos.z + (3f * Mathf.Cos(playerAngle)));
        Debug.DrawLine(playerPos, endPos, Color.white);

        // Positive
        float posAngle = playerAngle + posRange;
        endPos = new Vector3(playerPos.x + (3f * Mathf.Sin(posAngle)), playerPos.y, playerPos.z + (3f * Mathf.Cos(posAngle)));
        Debug.DrawLine(playerPos, endPos, Color.green);

        // Negative
        float negAngle = playerAngle - negRange;
        endPos = new Vector3(playerPos.x + (3f * Mathf.Sin(negAngle)), playerPos.y, playerPos.z + (3f * Mathf.Cos(negAngle)));
        Debug.DrawLine(playerPos, endPos, Color.red);


        // All enemies within range
        Collider[] enemiesInRange = Physics.OverlapSphere(playerPos, range, enemyMask);

        foreach (Collider e in enemiesInRange)
        {
            Vector3 enemyPos = e.gameObject.transform.position;
            float angle = Mathf.Atan2(enemyPos.x - playerPos.x, enemyPos.z - playerPos.z) * Mathf.Rad2Deg;

            endPos = new Vector3(playerPos.x + (3f * Mathf.Sin(angle)), playerPos.y, playerPos.z + (3f * Mathf.Cos(angle)));
            Debug.DrawLine(playerPos, endPos, Color.blue);

            if (angle >= negAngle && angle <= posAngle)
            {
                Debug.Log("Hit");
            }
        }
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 playerPos = transform.parent.position;
        //float playerAngle = Mathf.Atan2(mouseInput.x - playerPos.x, mouseInput.y - playerPos.z * Mathf.Rad2Deg) + WrapAngle(Camera.main.transform.eulerAngles.y);

        //Vector3 endPos = new Vector3(range * Mathf.Cos(playerAngle) + playerPos.x, 0, range * Mathf.Sin(playerAngle) + playerPos.z);

        //Debug.DrawLine(playerPos, endPos, Color.white);


        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging)
        {
            Attack();
        }

        if (isAttacking)
        {
            HitDetection();
            //RaycastDetect();
        }
    }
}
