using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private List<List<Vector3>> previousPosition;

    [HideInInspector]
    public bool isSwinging;

    bool isAttacking;
    public bool raycastHit = false;

    public RaycastHit hit;

    public WeaponDetect detect;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        DisableWeapon();
    }

    // Plays attacking animation
    void Attack()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging)
        {
            Attack();
        }

        if (isAttacking)
        {
            RaycastDetect();
        }
    }
}
