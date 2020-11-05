using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("MainScene_001_Arena");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
