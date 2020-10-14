using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetect : MonoBehaviour
{
    public Material redMat;
    public Material defaultMat;

    public float colourChangeDelay = 0.5f;
    float currentDelay = 0f;

    bool colourChangeCollision = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            colourChangeCollision = true;
            Debug.Log("Test");
            currentDelay = Time.time + colourChangeDelay;
        }
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
