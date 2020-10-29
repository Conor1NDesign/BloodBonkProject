using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    bool paused = false;
    GameObject gameOver;

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
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene_001_Arena");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        gameOver = GameObject.Find("/Canvas/GameOver");
    }
}
