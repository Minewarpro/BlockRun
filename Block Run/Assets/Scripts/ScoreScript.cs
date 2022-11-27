using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public int scoreValue = 0;
    public Text score;
    public Vector3 initScale;

    void Start()
    {
        initScale = transform.localScale;
        score = GetComponent<Text>();
        scoreValue = 0;
    }

    public void ResetScore()
    {
        scoreValue = 0;
    }
    
    void Update()
    {
        score.text = "" + scoreValue;
    }
}
