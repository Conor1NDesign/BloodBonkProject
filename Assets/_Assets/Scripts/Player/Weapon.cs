using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    public float damage = 10f;
    public float attackSpeed = 1f;
    public float attackSpeedCap = 2f;
    public float lungeDistance = 1f;

    [Header("Hit Detection")]
    public float range = 3f;
    LayerMask enemyMask;
    [Tooltip("How wide the angle/cone the attack will be")] public float hitDetectionRange = 70f;
	public GameObject bloodEffectPrefab;

    [HideInInspector] public bool isSwinging;

    List<Vector3> currentHardPointPos;
    List<Vector3> debugCurrentHardPointPos;

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

        currentHardPointPos = new List<Vector3>();
        debugCurrentHardPointPos = new List<Vector3>();
    }

    // Plays attacking animation
    void Attack()
    {
        player.hasLunged = false;
        isSwinging = true; // Enables animation in PlayerMovement script

        player.mouseInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        player.posOnScreen = Camera.main.WorldToScreenPoint(player.transform.position);
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < currentHardPointPos.Count - 5; i++)
        {
            Gizmos.DrawLine(currentHardPointPos[i], currentHardPointPos[i + 5]);
        }

        //for (int i = 0; i < debugCurrentHardPointPos.Count - 5; i++)
        //{
        //    Gizmos.DrawLine(debugCurrentHardPointPos[i], debugCurrentHardPointPos[i + 5]);
        //}

        //for (int i = 0; i < debugCurrentHardPointPos.Count; i++)
        //{
        //    Gizmos.DrawWireSphere(currentHardPointPos[i], 0.3f);
        //}
    }

    void Raycast()
    {
        // Gets HardPoints
        Transform hardPoints = transform.GetChild(2);

        // Remove Start HardPoints
        if (currentHardPointPos.Count > hardPoints.childCount * 3)
        {
            for (int i = 0; i < hardPoints.childCount; i++)
            {
                currentHardPointPos.RemoveAt(0);
            }
        }

        // Add Hardpoint position to list
        foreach (Transform child in hardPoints)
        {
            currentHardPointPos.Add(child.position);

            // Debug
            debugCurrentHardPointPos.Add(child.position);
        }

        for (int i = 0; i < currentHardPointPos.Count - hardPoints.childCount; i++)
        {
            RaycastHit hit;

            // Raycast check if it hits enemy
            if (Physics.Raycast(currentHardPointPos[i], currentHardPointPos[i + hardPoints.childCount], out hit, 1f, LayerMask.GetMask("Enemy")))
            {
                hit.collider.gameObject.GetComponentInParent<WeaponDetect>().TakeDamage(damage);
                stats.Lifesteal(damage);

                // Shake Camera
                StartCoroutine(camShake.Shake());
            }
        }
    }

    public void HitDetection()
    {
        //player.hasLunged = true;

        //Vector3 playerPos = transform.root.position;

        //// DEBUGGING
        //#region
        ////float playerAngle = transform.parent.eulerAngles.y * Mathf.Deg2Rad;

        ////// Player Angle
        ////Vector3 endPos = new Vector3(playerPos.x + (range * Mathf.Sin(playerAngle)), playerPos.y, playerPos.z + (range * Mathf.Cos(playerAngle)));
        ////Debug.DrawLine(playerPos, endPos, Color.yellow);

        ////// Positive
        ////float posAngle = playerAngle + posRange;
        ////endPos = new Vector3(playerPos.x + (range * Mathf.Sin(posAngle)), playerPos.y, playerPos.z + (range * Mathf.Cos(posAngle)));
        ////Debug.DrawLine(playerPos, endPos, Color.green);

        ////// Negative
        ////float negAngle = playerAngle - negRange;
        ////endPos = new Vector3(playerPos.x + (range * Mathf.Sin(negAngle)), playerPos.y, playerPos.z + (range * Mathf.Cos(negAngle)));
        ////Debug.DrawLine(playerPos, endPos, Color.red);
        //#endregion //Debugging

        //// All enemies within range
        //Collider[] enemiesInRange = Physics.OverlapSphere(playerPos, range, enemyMask);

        //bool enemyHit = false;

        //// Loop through all enemies
        //foreach (Collider e in enemiesInRange)
        //{
        //    // DEBUGGING
        //    //float angle = Vector3.Angle(transform.root.forward, e.gameObject.transform.position - playerPos);

        //    // Hit Detection cone if facing enemy
        //    if (Vector3.Angle(transform.root.forward, e.gameObject.transform.position - playerPos) < hitDetectionRange)
        //    {
        //        WeaponDetect detect = e.gameObject.GetComponentInParent<WeaponDetect>();
        //        if (bloodEffectPrefab != null)
        //            Instantiate(bloodEffectPrefab, detect.transform);
        //        detect.TakeDamage(damage);
        //        stats.Lifesteal(damage);

        //        enemyHit = true;
        //    }
        //}

        //if (enemyHit)
        //{
        //    StartCoroutine(camShake.Shake());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (isSwinging)
        {
            Raycast();
        }
        else
        {
            currentHardPointPos.Clear();
        }
    }
}
