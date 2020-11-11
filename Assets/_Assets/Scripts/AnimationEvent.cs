using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    PlayerMovement player;

    Weapon weapon;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();

        weapon = FindObjectOfType<Weapon>();
    }

    public void HitDetection()
    {
        weapon.HitDetection();
    }

    public void EndAttackAnimation()
    {
        player.animator.Play("Player_Idle");
        weapon.isSwinging = false;
        weapon.isAttacking = false;
    }
}
