using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    // Classes
    [HideInInspector] public EnemyAI enemy;
    Game gameManager;

    public float mass = 3f; // define the character mass
    public float force = 10f;

    Vector3 impact = Vector3.zero;

    float currentDelay;

    void Start()
    {
        enemy = GetComponent<EnemyAI>();
        gameManager = FindObjectOfType<Game>();
    }

    void FixedUpdate()
    {
        // apply the impact effect:
        if (impact.magnitude > 0.2f)
        {
            // Movement here
            enemy.transform.position += impact * Time.fixedDeltaTime;
        }
        // impact energy goes by over time:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    public void AddImpact(Vector3 dir)
    {
        dir = dir.normalized * force;

        impact += dir / mass;
    }

    public void TakeDamage(float damage)
    {
        if (currentDelay < Time.time)
        {
            currentDelay = Time.time + 0.5f; // Enemy invincible for 0.5secs

            enemy.TakeDamage(damage);
            gameManager.dmgDealt += damage;
        }
    }
}
