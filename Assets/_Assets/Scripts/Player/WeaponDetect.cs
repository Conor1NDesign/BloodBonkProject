using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    public Score score;

    public Material redMat;
    public Material defaultMat;

    public float colourChangeDelay = 0.5f;
    float currentDelay = 0f;

    bool colourChangeCollision = false;

    public Weapon weapon;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            score.UpdateScore();
            colourChangeCollision = true;
            currentDelay = Time.time + colourChangeDelay;
        }
    }

    public void TakeDamage()
    {
        score.UpdateScore();
        colourChangeCollision = true;
        currentDelay = Time.time + colourChangeDelay;
    }

    void Update()
    {
        if (colourChangeCollision)
        {
            transform.GetComponent<MeshRenderer>().material = redMat;
            if (Time.time > currentDelay)
            {
                transform.GetComponent<MeshRenderer>().material = defaultMat;
                colourChangeCollision = false;
            }
        }
    }
}
