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
	public GameObject bloodEffectPrefab;

    [Header("Weapon Sounds")]
    public AudioSource hitSound;
    public AudioSource swingSound;

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

        currentHardPointPos = new List<Vector3>();
        debugCurrentHardPointPos = new List<Vector3>();
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

    // Plays attacking animation
    void Attack()
    {
        player.hasLunged = false;
        isSwinging = true; // Enables animation in PlayerMovement script

        if (swingSound != null)
            swingSound.Play();

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

        // Remove Starting HardPoints
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
                WeaponDetect detect = hit.collider.gameObject.GetComponentInParent<WeaponDetect>();
                detect.TakeDamage(stats.godMode ? damage * 1000000.0f : damage);
                detect.Lifesteal(damage);

                // Knockback
                Vector3 dir = detect.enemy.transform.position - player.transform.position;
                dir.y = 0;
                detect.AddImpact(dir);

                // Shake Camera
                StartCoroutine(camShake.Shake());
            }
        }
    }
}
