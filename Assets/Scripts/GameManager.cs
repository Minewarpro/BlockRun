using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public EndScreen EndScreen;
    public Player Player;

    [SerializeField] Text ScoreText;
    [SerializeField] Text TimerText;

    [SerializeField] TimerScript timerScript;
    private ScoreScript scoreScript;
    private HighScoreScript highScoreScript;
    private HighScoreScriptHard highScoreScriptHard;
    private Color currentPlayerColor;

    [SerializeField] GameObject player;
    [SerializeField] StartScreen startScreen;

    [SerializeField] List<Material> materialsList = new List<Material>();
    [SerializeField] List<Material> EasyMaterialsList = new List<Material>();
    [SerializeField] List<Material> MediumMaterialsList = new List<Material>();
    [SerializeField] List<Material> HardMaterialsList = new List<Material>();

    public string targetTag = "Target";

    GameObject[] targets;

    public bool canPlay = false;
    public bool isEasyMode = false;
    public bool isHardMode = false;

    
    private void Awake()
    {
        timerScript = FindObjectOfType<TimerScript>();
        targets = GameObject.FindGameObjectsWithTag(targetTag);
        scoreScript = FindObjectOfType<ScoreScript>();
        highScoreScript = FindObjectOfType<HighScoreScript>();
        highScoreScriptHard = FindObjectOfType<HighScoreScriptHard>();

        
    }

  
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        
    }

    public void RestartGame()
    {
        
        timerScript.ResetTimer();
        scoreScript.ResetScore();
        TimerText.gameObject.SetActive(true);
        ScoreText.gameObject.SetActive(true);
        timerScript.timerImage.gameObject.SetActive(true);
        startScreen.ResetTimer();
        timerScript.timerImage.GetComponent<Image>().color = Color.white;
        startScreen.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        player.transform.position = new Vector2(0, 0);
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        if (isEasyMode)
        {
            EndScreen.Setup(scoreScript.scoreValue, highScoreScript.highScore, highScoreScript.initHighScore);
            highScoreScript.initHighScore = highScoreScript.highScore;
        }else if (isHardMode)
        {
            EndScreen.Setup(scoreScript.scoreValue, highScoreScriptHard.highScore, highScoreScriptHard.initHighScore);
            highScoreScriptHard.initHighScore = highScoreScriptHard.highScore;
        }

        TimerText.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);
        canPlay = false;
    }

    public void ChooseMode()
    {
        if (isEasyMode)
        {
            materialsList = EasyMaterialsList;
        }
        else if (isHardMode)
        {
            materialsList = HardMaterialsList;
        }

        ChangeColor();
    }

    public void ChangeColor()
    {

        int i = 0;

        materialsList.Sort((a, b) => 1 - 2 * Random.Range(0, materialsList.Count));
        player.GetComponent<MeshRenderer>().material = materialsList[0];
        currentPlayerColor = player.GetComponent<MeshRenderer>().material.color;




        materialsList.Sort((a, b) => 1 - 2 * Random.Range(0, materialsList.Count));


        // Change la couleur des targets 
        foreach (GameObject target in targets)
        {
            target.GetComponent<MeshRenderer>().material = materialsList[i];
            i += 1;
        }

    }

    private void Update()
    {

        if (isEasyMode)
        {
            if (scoreScript.scoreValue > highScoreScript.highScore)
            {
                highScoreScript.highScore = scoreScript.scoreValue;
                PlayerPrefs.SetInt("highscore", highScoreScript.highScore);
            }
            highScoreScript.highScoreText.text = "" + highScoreScript.highScore;
        }else if (isHardMode)
        {
            if (scoreScript.scoreValue > highScoreScriptHard.highScore)
            {
                highScoreScriptHard.highScore = scoreScript.scoreValue;
                PlayerPrefs.SetInt("highscoreHard", highScoreScriptHard.highScore);
            }
            highScoreScriptHard.highScoreText.text = "" + highScoreScriptHard.highScore;
        }
        
        player.GetComponent<TrailRenderer>().startColor = currentPlayerColor;
        player.GetComponent<TrailRenderer>().endColor = currentPlayerColor;
    }
}
