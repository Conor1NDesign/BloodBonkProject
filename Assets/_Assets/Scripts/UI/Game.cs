﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject weaponHolder;
    public GameObject[] weapons;

    public GameObject weaponPrefab;

    // Classes
    Score score;

    // Components
    Button[] buttons;
    [Header("Highscore")]
    public Text yourScore;
    public Text bestScore;

    int highscore;
    string weapon;

    void Awake()
    {
        
        SelectWeapon();
    }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore", highscore);
        

        score = FindObjectOfType<Score>();
        buttons = gameOver.GetComponentsInChildren<Button>();
    }

    void SelectWeapon()
    {
        weapon = PlayerPrefs.GetString("Weapon");

        if (weapon == "")
        {
            weaponPrefab = Instantiate(weapons[0]);
            weaponPrefab.transform.parent = weaponHolder.transform;
        }
        else if (weapon == "Kanabo")
        {
            weaponPrefab = Instantiate(weapons[0]);
            weaponPrefab.transform.parent = weaponHolder.transform;
        }
        else if (weapon == "Katana")
        {
            weaponPrefab = Instantiate(weapons[1]);
            weaponPrefab.transform.parent = weaponHolder.transform;
        }
        else if (weapon == "Naginata")
        {
            weaponPrefab = Instantiate(weapons[2]);
            weaponPrefab.transform.parent = weaponHolder.transform;
        }
    }

    public void GameOver()
    {
        // GameOver On
        gameOver.SetActive(true);
        Time.timeScale = 0f;

        buttons[0].onClick.AddListener(PlayAgain);
        buttons[1].onClick.AddListener(QuitGame);

        // Preview Score
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