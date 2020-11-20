using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    PlayerMovement player;
    Game gameManager;
    Weapon weapon;

    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        gameManager = FindObjectOfType<Game>();
        weapon = FindObjectOfType<Weapon>();
    }

    void GameOver()
    {
        gameManager.GameOver();
    }

    void Lunged()
    {
        player.hasLunged = true;
    }

    public void EndAttackAnimation()
    {
        player.animator.speed = 1f;
        weapon.isSwinging = false;
        player.isAttacking = false;
    }
}
