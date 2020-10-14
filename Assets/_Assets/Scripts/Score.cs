using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text scoreText;
    private int currentScore;
    public int increaseScore;

    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    public void UpdateScore()
    {
        currentScore += increaseScore;
        scoreText.text = currentScore.ToString();
    }
}
