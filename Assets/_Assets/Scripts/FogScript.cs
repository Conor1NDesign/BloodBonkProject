using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    Vector3 myVector;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        myVector = new Vector3 (Player.transform.position.x, transform.position.y, Player.transform.position.z);
        gameObject.transform.position = myVector;
    }
}
