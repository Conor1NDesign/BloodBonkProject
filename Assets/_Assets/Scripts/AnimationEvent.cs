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

    public void EndAttackAnimation()
    {
        player.animator.speed = 1f;
        weapon.isSwinging = false;
        player.isAttacking = false;
    }
}
