using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Classes
    public GameObject gameOver;
    Score score;

    // Components
    Button[] buttons;
    public Text yourScore;
    public Text bestScore;

    int highscore;

    void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore", highscore);

        score = FindObjectOfType<Score>();
        buttons = gameOver.GetComponentsInChildren<Button>();
    }

    public void GameOver()
    {
        // GameOver On
        gameOver.SetActive(true);
        Time.timeScale = 0f;

        buttons[0].onClick.AddListener(PlayAgain);
        buttons[1].onClick.AddListener(QuitGame);

        PreviewScore();
    }

    void PreviewScore()
    {
        yourScore.text += score.currentScore.ToString();

        if (score.currentScore > highscore)
        {
            PlayerPrefs.SetInt("HighScore", score.currentScore);
            bestScore.text += score.currentScore.ToString() + "  *(New)*";
        }
        else
        {
            bestScore.text += highscore.ToString();
        }
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
