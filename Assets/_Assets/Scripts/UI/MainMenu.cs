using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject[] weapons;

    public void Kanabo()
    {
        

        LoadScene();
    }

    public void Katana()
    {


        LoadScene();
    }

    public void Naginata()
    {


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
