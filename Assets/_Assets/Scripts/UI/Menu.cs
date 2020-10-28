using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    bool paused = false;
    public GameObject gameOver;

    public bool toggleMenu()
    {
        if (Time.timeScale == 0f)
        {
            gameOver.SetActive(false);
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            gameOver.SetActive(true);
            Time.timeScale = 0f;
            return (true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = toggleMenu();
        }
    }
}
