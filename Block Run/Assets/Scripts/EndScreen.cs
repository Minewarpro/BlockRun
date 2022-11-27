using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class EndScreen : MonoBehaviour
{
    public Text pointsText;
    public Text scoreText;
    [SerializeField] GameManager gameManager;
    TimerScript timerScript;

    private void Awake()
    {
        timerScript = FindObjectOfType<TimerScript>();

    }

    public void Setup(int score, int highScore, int initHighScore)
    {
        if (highScore > initHighScore)
        {
            Debug.Log("test");
            scoreText.text = "New \n High Score";
            scoreText.fontSize = 95;
            pointsText.transform.localPosition = new Vector3 (0, 1.46f, 0);


            pointsText.transform.DOScaleX(pointsText.transform.localScale.x + 0.001f, 0.2f)
                .SetEase(Ease.InCirc)
                .SetLoops(-1, LoopType.Yoyo);
            pointsText.transform.DOScaleY(pointsText.transform.localScale.y + 0.001f, 0.2f)
                .SetEase(Ease.InCirc)
                .SetLoops(-1, LoopType.Yoyo);

            scoreText.transform.DOScaleX(scoreText.transform.localScale.x + 0.001f, 0.2f)
                .SetEase(Ease.InCirc)
                .SetLoops(-1, LoopType.Yoyo);
            scoreText.transform.DOScaleY(scoreText.transform.localScale.y + 0.001f, 0.2f)
                .SetEase(Ease.InCirc)
                .SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            DOTween.Kill(scoreText.transform);
            DOTween.Kill(pointsText.transform);
            scoreText.text = "Score :";
            scoreText.fontSize = 120;
            pointsText.transform.localPosition = new Vector3(0, 2, 0);

        }
        pointsText.text = "" + score.ToString();
        gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        timerScript.timerImage.SetActive(true);
        gameObject.SetActive(false);
        gameManager.RestartGame();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Game");
    }
}
