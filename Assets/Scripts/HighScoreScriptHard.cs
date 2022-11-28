using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScriptHard : MonoBehaviour
{
    public int highScore;
    public int initHighScore;

    public Text highScoreText;

    private void Awake()
    {
        highScoreText = GetComponent<Text>();
        highScore = PlayerPrefs.GetInt("highscoreHard", highScore);
        highScoreText.text = "" + highScore;

    }

    void Start()
    {
        initHighScore = highScore;
    }

    void Update()
    {
    }
}
