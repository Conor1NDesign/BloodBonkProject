using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    //public Score score;

    public Material redMat;
    public Material defaultMat;

    public float colourChangeDelay = 0.5f;
    float currentDelay = 0f;

    bool colourChangeCollision = false;

    public void TakeDamage()
    {
        //score.UpdateScore();
        colourChangeCollision = true;
        currentDelay = Time.time + colourChangeDelay;

        Debug.Log("Hit");
    }

    void Update()
    {
        if (colourChangeCollision)
        {
            transform.GetComponentInChildren<MeshRenderer>().material = redMat;
            if (Time.time > currentDelay)
            {
                transform.GetComponentInChildren<MeshRenderer>().material = defaultMat;
                colourChangeCollision = false;
            }
        }
    }
}
