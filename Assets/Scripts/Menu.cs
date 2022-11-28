using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Menu : MonoBehaviour
{

    [SerializeField] StartScreen startScreen;
    [SerializeField] Button volumeButton;
    [SerializeField] Button vibrateButton;

    [SerializeField] Sprite NoneVolumeButton;
    [SerializeField] Sprite OnVolumeButton;
    
    [SerializeField] Sprite NoneVibrateButton;
    [SerializeField] Sprite OnVibrateButton;
    
    [SerializeField] GameObject PlayHardButton;
    [SerializeField] GameObject Play;

    [SerializeField] GameObject BestScoreHard;
    [SerializeField] GameObject BestScore;
    
    [SerializeField] GameObject TitleHard;
    [SerializeField] GameObject Title;

    [SerializeField] AudioSource Click;


    private GameManager gameManager;
    private int isMute;
    private bool isHard = false;
    private int isVibration;
    Transform button;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        gameManager.isEasyMode = true;
        button = gameObject.transform.GetChild(5).GetChild(0);


        isMute = PlayerPrefs.GetInt("Mute", isMute);
        if (isMute == 1)
        {
            volumeButton.GetComponent<Image>().sprite = NoneVolumeButton;
        }
        else if (isMute == 0)
        {
            volumeButton.GetComponent<Image>().sprite = OnVolumeButton;
        }

        isVibration = PlayerPrefs.GetInt("Vibrate", isVibration);
        if (isVibration == 1)
        {
            vibrateButton.GetComponent<Image>().sprite = NoneVibrateButton;
        }
        else if (isVibration == 0)
        {
            vibrateButton.GetComponent<Image>().sprite = OnVibrateButton;
        }
    }

    void Update()
    {
        
    }


    public void PlayButton()
    {
        gameManager.ChooseMode();
        gameObject.SetActive(false);
        startScreen.gameObject.SetActive(true);
    }
    
    public void HardButton()
    {
        if (!isHard)
        {
            gameManager.isHardMode = true;
            gameManager.isEasyMode = false;
            isHard = true;
            
            button.DOLocalMoveX(0.778f, 0.2f);
            button.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1.27f, 0.63f), 0.2f);

            PlayHardButton.transform.DOLocalMoveX(0.07f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.1f); 
            Play.transform.DOLocalMoveX(5.38f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.1f); 

            BestScoreHard.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutBack).SetDelay(0.05f);
            BestScore.transform.DOMoveX(5, 0.5f).SetEase(Ease.OutBack).SetDelay(0.05f);

            TitleHard.transform.DOLocalMoveX(0.03f, 0.5f).SetEase(Ease.OutBack);
            Title.transform.DOMoveX(5, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            gameManager.isHardMode = false;
            gameManager.isEasyMode = true;
            isHard = false;

            button.DOLocalMoveX(-0.609f, 0.2f);
            button.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1.6f, 0.63f), 0.2f);

            PlayHardButton.transform.DOLocalMoveX(-5.31f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.1f);
            Play.transform.DOLocalMoveX(0.07f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.1f);

            BestScoreHard.transform.DOLocalMoveX(-5.24f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.05f);
            BestScore.transform.DOMoveX(0f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.05f);
            
            TitleHard.transform.DOLocalMoveX(-5.37f, 0.5f).SetEase(Ease.OutBack);
            Title.transform.DOMoveX(0.03f, 0.5f).SetEase(Ease.OutBack);
        }
    }


    public void VolumeButton()
    {
        Click.Play();

        if (isMute == 0)
        {
            volumeButton.GetComponent<Image>().sprite = NoneVolumeButton;
            PlayerPrefs.SetInt("Mute", 1);
            isMute = PlayerPrefs.GetInt("Mute", isMute);
            AudioListener.volume = 0;
        }
        else if (isMute == 1)
        {
            volumeButton.GetComponent<Image>().sprite = OnVolumeButton;
            PlayerPrefs.SetInt("Mute", 0);
            isMute = PlayerPrefs.GetInt("Mute", isMute);
            AudioListener.volume = 1;
        }
    }

    public void VibrationButton()
    {
        Click.Play();
        if (isVibration == 0)
        {
            vibrateButton.GetComponent<Image>().sprite = NoneVibrateButton;
            PlayerPrefs.SetInt("Vibrate", 1);
            isVibration = PlayerPrefs.GetInt("Vibrate", isVibration);
        }
        else if (isVibration == 1)
        {
            vibrateButton.GetComponent<Image>().sprite = OnVibrateButton;
            PlayerPrefs.SetInt("Vibrate", 0);
            isVibration = PlayerPrefs.GetInt("Vibrate", isVibration);
        }

    }

}

