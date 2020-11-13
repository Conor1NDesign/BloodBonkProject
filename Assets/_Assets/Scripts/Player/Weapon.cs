using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    public float damage = 10f;
    public float attackSpeed = 1f;

    [Header("Hit Detection")]
    public float range = 3f;
    LayerMask enemyMask;
    [Tooltip("How wide the angle/cone the attack will be")] public float hitDetectionRange = 70f;

    [HideInInspector]
    public bool isSwinging;
    
    // Classes
    CameraShake camShake;
    PlayerStats stats;
    PlayerMovement player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        camShake = FindObjectOfType<CameraShake>();
        stats = FindObjectOfType<PlayerStats>();
        enemyMask = LayerMask.GetMask("Enemy");
    }

    // Plays attacking animation
    void Attack()
    {
        isSwinging = true; // Enables animation in PlayerMovement script

        player.mouseInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        player.posOnScreen = Camera.main.WorldToScreenPoint(player.transform.position);
    }

    public void HitDetection()
    {
        Vector3 playerPos = transform.root.position;

        // DEBUGGING
        #region
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
        #endregion //Debugging

        // All enemies within range
        Collider[] enemiesInRange = Physics.OverlapSphere(playerPos, range, enemyMask);

        bool enemyHit = false;

        // Loop through all enemies
        foreach (Collider e in enemiesInRange)
        {
            // DEBUGGING
            //float angle = Vector3.Angle(transform.root.forward, e.gameObject.transform.position - playerPos);

            // Hit Detection cone if facing enemy
            if (Vector3.Angle(transform.root.forward, e.gameObject.transform.position - playerPos) < hitDetectionRange)
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
    }
}
