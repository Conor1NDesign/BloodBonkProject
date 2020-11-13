using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTest : MonoBehaviour
{
    public GameObject enemy;
    Renderer m_ObjectRenderer;


    // Start is called before the first frame update
    void Start()
    {
        m_ObjectRenderer = enemy.GetComponent<Renderer>();
        m_ObjectRenderer.material.SetColor("_EmissionColor", Color.black);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("g"))
        {
            Debug.Log("flash");
            m_ObjectRenderer.material.SetColor("_EmissionColor", Color.red * 311);
        }
        else
        {
            Debug.Log("not flash");
            m_ObjectRenderer.material.SetColor("_EmissionColor", Color.black);

        }
    }
}
