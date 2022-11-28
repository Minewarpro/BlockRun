using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    public float currentTime = 0f;
    public float startingTime = 3f;
    public bool startTimer = false;
    [SerializeField] Text timer;
    [SerializeField] AudioSource countDownAudio;


    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        currentTime = startingTime;
        countDownAudio.Play();
    }

    public void ResetTimer()
    {
        currentTime = startingTime;
        timer.GetComponent<Text>().text = currentTime.ToString("0");
        countDownAudio.Play();
    }

    private void Timer()
    {
        if (currentTime <= 0.5f && currentTime > -0.5f)
        {
            timer.GetComponent<Text>().text = "GO";
        }
        else if (currentTime > 0.5f)
        {
            timer.GetComponent<Text>().text = currentTime.ToString("0");
        }
        else
        {
            gameObject.SetActive(false);
            gameManager.canPlay = true;
        }

        currentTime -= 1 * Time.deltaTime;
    }

    void Update()
    { 
        if (gameObject.activeSelf == true)
        {
            Timer();
        }   
    }
}
