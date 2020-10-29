using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Classes
    public GameObject gameOver;

    Button[] buttons;

    void Start()
    {
        buttons = gameOver.GetComponentsInChildren<Button>();
    }

    public void GameOver()
    {
        // GameOver On
        gameOver.SetActive(true);
        Time.timeScale = 0f;

        buttons[0].onClick.AddListener(PlayAgain);
        buttons[1].onClick.AddListener(QuitGame);
    }

    // Reset and Unpause game
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene_001_Arena");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
