using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteKey("Weapon");
    }

    public void Kanabo()
    {
        PlayerPrefs.SetString("Weapon", "Kanabo");

        LoadScene();
    }

    public void Katana()
    {
        PlayerPrefs.SetString("Weapon", "Katana");

        LoadScene();
    }

    public void Naginata()
    {
        PlayerPrefs.SetString("Weapon", "Naginata");

        LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene("MainScene_001_Arena");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
