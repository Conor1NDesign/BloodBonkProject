using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("Menu")]
    public GameObject gameOver;
    public GameObject pause;

    [Header("Weapon")]
    public GameObject weaponHolder;
    public GameObject[] weapons;

    // Classes
    Score score;

    // Components
    Button[] buttons;
    [Header("Highscore")]
    public Text yourScore;
    public Text bestScore;

    [Header("Debug (NO TOUCH)")]
    public GameObject weaponPrefab;

    int highscore;
    string weapon;
    bool isPaused = false;

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

    void Update()
    {
        Pause();
    }

    // Pause Menu
    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;

            Time.timeScale = 0f;
            pause.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            isPaused = false;

            Time.timeScale = 1f;
            pause.SetActive(false);
        }
    }

    void SelectWeapon()
    {
        weapon = PlayerPrefs.GetString("Weapon");

        if (weapon == "")
        {
            weaponPrefab = Instantiate(weapons[0]);
            WeaponSetting(weaponPrefab);
        }
        else if (weapon == "Kanabo")
        {
            weaponPrefab = Instantiate(weapons[0]);
            WeaponSetting(weaponPrefab);
        }
        else if (weapon == "Katana")
        {
            weaponPrefab = Instantiate(weapons[1]);
            WeaponSetting(weaponPrefab);
        }
        else if (weapon == "Naginata")
        {
            weaponPrefab = Instantiate(weapons[2]);
            WeaponSetting(weaponPrefab);
        }
    }

    void WeaponSetting(GameObject obj)
    {
        obj.transform.parent = weaponHolder.transform;
        obj.transform.localPosition = weaponHolder.transform.localPosition;
        obj.transform.localRotation = weaponHolder.transform.localRotation;
    }

    public void GameOver()
    {
        // GameOver On
        gameOver.SetActive(true);
        Time.timeScale = 0f;

        buttons[0].onClick.AddListener(PlayAgain);
        buttons[1].onClick.AddListener(MainMenu);

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

    // Buttons
    #region
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    // Reset and Unpause game
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene_001_Arena");
    }
    #endregion
}
