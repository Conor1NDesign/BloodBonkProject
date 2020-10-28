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
    public float range = 3f;
    public LayerMask enemyMask;
    Vector2 mouseInput;
    // Debugging
    public float hitDetectionRange = 40f;

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
        //transform.GetComponent<BoxCollider>().enabled = true;
        //isAttacking = true;
        HitDetection();
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
        float playerAngle = transform.parent.eulerAngles.y * Mathf.Deg2Rad;

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


        // All enemies within range
        Collider[] enemiesInRange = Physics.OverlapSphere(playerPos, range, enemyMask);

        foreach (Collider e in enemiesInRange)
        {
            float angle = Vector3.Angle(transform.parent.forward, e.gameObject.transform.position - playerPos);
            Debug.Log(angle);

            if (Vector3.Angle(transform.parent.forward, e.gameObject.transform.position - playerPos) < hitDetectionRange)
            {
                detect = e.gameObject.GetComponent<WeaponDetect>();
                detect.TakeDamage();
            }

            //Vector3 enemyPos = e.gameObject.transform.position;
            //float angle = Mathf.Atan2(enemyPos.x - playerPos.x, enemyPos.z - playerPos.z);

            //endPos = new Vector3(playerPos.x + (range * Mathf.Sin(angle)), playerPos.y, playerPos.z + (range * Mathf.Cos(angle)));
            //Debug.DrawLine(playerPos, endPos, Color.blue);

            //angle = (angle * Mathf.Rad2Deg + 360) % 360;
            //posAngle *= Mathf.Rad2Deg;
            //negAngle *= Mathf.Rad2Deg;

            //negAngle %= 360;

            //posAngle %= 360;

            //Debug.Log(negAngle + " > " + angle + " < " + posAngle);
            //if (angle >= negAngle && angle <= posAngle)
            //{
            //    Debug.Log("Hit");
            //    detect = e.gameObject.GetComponent<WeaponDetect>();
            //    detect.TakeDamage();
            //}
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
