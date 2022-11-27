using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TimerScript : MonoBehaviour
{
    public float currentTime = 0f;
    public float startingTime = 30f;
    Text timer;
    [SerializeField] public GameObject timerImage;
    [SerializeField] public int criticalNumber = 5;

    [SerializeField] AudioSource alertSound;

    private GameManager gameManager;
    private bool isTweening;
    private bool isAlertSoundEnable;



    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        timer = GetComponent<Text>();
    }

    void Start()
    {
        currentTime = startingTime;
        isAlertSoundEnable = true;
    }

    public void ResetTimer()
    {
        currentTime = startingTime;
        timer.text = currentTime.ToString("0");
        timer.color = Color.white;
        isTweening = false;
        timerImage.transform.position = new Vector2(timer.transform.position.x - 1.3f, timerImage.transform.position.y);
        isAlertSoundEnable = true;
    }

    private void Timer()
    {
        if (currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
        }
        else
        {
            timerImage.SetActive(false);
            currentTime = 0f;
            gameManager.EndGame();
        }

        if (currentTime <= criticalNumber + 0.5f)
        {
            timer.color = Color.red;
            timerImage.GetComponent<Image>().color = Color.red;

            if (isAlertSoundEnable)
            {
                isAlertSoundEnable = false;
                alertSound.Play();
            }
        }
    }

    void Update()
    {
        if (currentTime <= 9.5)
        {
            timerImage.transform.position = new Vector2(
                timer.transform.position.x - 0.9f,
                timerImage.transform.position.y);
        } else
        {
            timerImage.transform.position = new Vector2(
                timer.transform.position.x - 1.3f,
                timerImage.transform.position.y);
        }

        if (gameManager.canPlay)
        {
            Timer();
        }

        if (currentTime <= criticalNumber + 0.5f && !isTweening)
        {
            Vector3 scaleToTween = new Vector3(transform.localScale.x + 0.005f, transform.localScale.y + 0.005f, transform.localScale.z);
            transform.DOScale(scaleToTween, 0.5f).SetLoops(criticalNumber * 2, LoopType.Yoyo);
            isTweening = true;
        }

        timer.text = currentTime.ToString("0");
    }
}
