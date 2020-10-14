using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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
        transform.GetComponent<BoxCollider>().enabled = true;
    }

    public void DisableWeapon()
    {
        transform.GetComponent<Renderer>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
}
