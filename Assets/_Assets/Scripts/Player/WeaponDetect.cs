using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    // Classes
    [HideInInspector] public EnemyAI enemy;
    Game gameManager;
    PlayerStats stats;
    Weapon weapon;

    public float mass = 3f; // define the character mass
    public float force = 10f;

    Vector3 impact = Vector3.zero;

    float damageDelay;
    float knockbackDelay;
    float lifestealDelay;

    void Start()
    {
        enemy = GetComponent<EnemyAI>();
        gameManager = FindObjectOfType<Game>();
        stats = FindObjectOfType<PlayerStats>();
        weapon = FindObjectOfType<Weapon>();
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
        if (knockbackDelay < Time.time)
        {
            knockbackDelay = Time.time + 0.5f;

            dir = dir.normalized * force;

            impact += dir / mass;
        }
    }

    public void Lifesteal(float damage)
    {
        if (lifestealDelay < Time.time)
        {
            lifestealDelay = Time.time + 0.5f; // Enemy invincible for 0.5secs

            if (stats.currentHealth < stats.maxHealth)
            {
                float lifeStolen = damage * stats.lifesteal;

                stats.currentHealth += lifeStolen;
                gameManager.lifeStolen += (int)lifeStolen;
            }

            if (stats.currentHealth > stats.maxHealth)
            {
                float reduce = stats.currentHealth - stats.maxHealth;
                gameManager.lifeStolen -= (int)reduce;

                stats.currentHealth = stats.maxHealth;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (damageDelay < Time.time)
        {
            damageDelay = Time.time + 0.5f; // Enemy invincible for 0.5secs

            weapon.hitSound.Play();

            enemy.TakeDamage(damage);
            gameManager.dmgDealt += (int)damage;

            if (weapon.bloodEffectPrefab != null)
                Instantiate(weapon.bloodEffectPrefab, transform);
        }
    }
}
