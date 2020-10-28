using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainScene_001_Arena");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = toggleMenu();
        }
    }
}
