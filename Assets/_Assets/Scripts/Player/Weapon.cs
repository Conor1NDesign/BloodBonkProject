using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] emptyPosition;

    private List<Vector3[]> previousPosition;

    bool isAttacking;


    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        DisableWeapon();
    }

    void Attack()
    {
        transform.GetComponent<Renderer>().enabled = true;
        animator.Play("Attack");
    }

    public void EnableWeapon()
    {
        previousPosition = new List<Vector3[]>();
        transform.GetComponent<BoxCollider>().enabled = true;
        isAttacking = true;
    }

    public void DisableWeapon()
    {
        transform.GetComponent<Renderer>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        isAttacking = false;
    }

    void SetPreviousPosition()
    {

        Vector3[] position = { emptyPosition[0].transform.position,
                               emptyPosition[1].transform.position, 
                               emptyPosition[2].transform.position, 
                               emptyPosition[3].transform.position,
                               emptyPosition[4].transform.position,
                               emptyPosition[5].transform.position,};

        previousPosition.Add(position);
    }

    private void DebugLine()
    {
        SetPreviousPosition();

        for (int l = 0; l < previousPosition.Count - 1; l++)
        {
            for (int i = 0; i < emptyPosition.Length; i++)
            {
                Debug.DrawLine(previousPosition[l][i], previousPosition[l + 1][i], Color.green);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

        if (isAttacking)
        {
            DebugLine();
        }
    }
}
