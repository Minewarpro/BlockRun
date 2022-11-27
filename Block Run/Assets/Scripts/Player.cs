using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Cache
    private Rigidbody rb;
    private GameManager gameManager;
    private ScoreScript scoreScript;

    private Collider target;

    [SerializeField] float moveSpeed = 0.01f;
    [SerializeField] AudioSource getPoint;
    [SerializeField] AudioSource losePoint;


    [SerializeField] GameObject targetUp;
    [SerializeField] GameObject targetDown;
    [SerializeField] GameObject targetRight;

    [SerializeField] GameObject PulsePrefab;

    [SerializeField] Image blackScreen;



    // Variable private

    private bool isCollideRetargetUp;
    private bool isCollideRetargetDown;
    private bool canMove = true;
    private bool isMove = false;
    private bool isScreenPressed;
    private bool isGetingPoint = false;
    private float directionX;
    private float directionY;
    [SerializeField]  private float vitesseMax = 1;
    [SerializeField]  private float vitesseMin = 0;
    private Vector3 dir;
    private float currentVitesse;

    private string targetTag = "Target";

    private float targetsScaleXUp;
    private float targetsScaleYUp;
    private float targetsScaleZUp;

    private float targetsScaleXRight;
    private float targetsScaleYRight;
    private float targetsScaleZRight;

    private float initScaleX;
    private float initScaleY;
    private float initScaleZ;

    private float angleCube;

    //Variable public
    public bool isCollide;
    [SerializeField] ShakeData MyShake;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        scoreScript = FindObjectOfType<ScoreScript>();
    }

    void Start()
    {
        targetsScaleXUp = targetUp.transform.localScale.x;
        targetsScaleYUp = targetUp.transform.localScale.y;
        targetsScaleZUp = targetUp.transform.localScale.z;
        
        targetsScaleXRight = targetRight.transform.localScale.x;
        targetsScaleYRight = targetRight.transform.localScale.y;
        targetsScaleZRight = targetRight.transform.localScale.z;

        initScaleX = transform.localScale.x;
        initScaleY = transform.localScale.y;
        initScaleZ = transform.localScale.z;
    }

    private void OnTriggerStay(Collider other)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other;
        // Si le player touche une target (cube de couleur exterieur)
        if (other.tag == targetTag && !isCollide)
            {
                // Detecte si même couleur
                if (GetComponent<MeshRenderer>().material.name == other.GetComponent<MeshRenderer>().material.name)
                {
                    // Donne un point
                    scoreScript.scoreValue += 1;
                    scoreScript.score.transform.DOScale(scoreScript.initScale * 1.2f, 0.1f).SetLoops(2, LoopType.Yoyo);
                    getPoint.Play();
                    isGetingPoint = true;
                }
                else
                {
                    if (scoreScript.scoreValue > 0)
                    {
                        // Enlève un point
                        scoreScript.scoreValue -= 1;
                    }
                    
                    LoosePointEffect();

                }
                
                isCollide = true;

                // Remet le player au centre
                ResetSleeve();
            }

        
    }

    public IEnumerator VibrateDuration()

    {
        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            yield return new WaitForSeconds(0.2f);
            Handheld.Vibrate();
        }
        
    }

    private void MovePlayer()
    {

        if (Input.touchCount > 0)
        {
                Touch touch = Input.GetTouch(0);

 
            if (canMove)
            {


                if (touch.phase == TouchPhase.Began)
                {
                    GetComponent<TrailRenderer>().time = 0.1f;
                    transform.DOScale(new Vector3 (initScaleX + 0.3f, initScaleY + 0.3f, initScaleZ - 0.6f), 0.2f);
                    directionY = 0;
                    directionX = 0;
                    dir = new Vector2(directionX, directionY);
                    rb.velocity = Vector3.zero;
                }

                
                if (touch.phase == TouchPhase.Moved)
                    {
                        isScreenPressed = true;
                        isMove = true;
                        rb.velocity = Vector3.zero;

                        transform.position = new Vector2(
                            transform.position.x + touch.deltaPosition.x * moveSpeed,
                            transform.position.y + touch.deltaPosition.y * moveSpeed);

                       angleCube = AngleCalculator(new Vector2(touch.deltaPosition.x, touch.deltaPosition.y));

                       directionY = touch.deltaPosition.y;
                       directionX = touch.deltaPosition.x;

                       if (angleCube >= 45 && angleCube <= 90 || angleCube <= -45 && angleCube >= -90)
                       {
                            directionX = 0;
                       }
                        else
                        {
                        directionY = 0;
                        }  


                        if (directionX > 50)
                        {
                            directionX = 50;
                        }else if (directionX < -50)
                        {
                            directionX = -50;
                        }

                        if (directionY > 50)
                        {
                            directionY = 100;
                        }
                        else if (directionY < -50)
                        {
                            directionY = -100;
                        }

                    dir = new Vector2(directionX, directionY);
                    float magnitude = dir.magnitude;
                    


                    if (magnitude > 50)
                    {
                        magnitude = 50;
                    } else if (magnitude < 15 && magnitude > 5.6f)
                    {
                        magnitude = 15;
                    }else if (magnitude < 5.6f)
                    {
                        magnitude = 0;
                    }
                    currentVitesse = Mathf.Lerp(vitesseMin, vitesseMax, magnitude / 50);

                   
                }

                
                if (touch.phase == TouchPhase.Ended)
                {
                    transform.DOScale(new Vector3(initScaleX - 0.3f, initScaleY - 0.3f, initScaleZ + 0.6f), 0.2f);

                    isMove = false;
                    if(!isCollideRetargetDown && !isCollideRetargetUp)
                    {
                        rb.velocity = dir * currentVitesse;
                    }

                    dir = new Vector2(0, 0);

                }
                if (isCollideRetargetDown || isCollideRetargetUp)
                {
                    rb.velocity = Vector2.zero;
                }

            }

            if (touch.phase == TouchPhase.Ended)
            {
                canMove = true;
                isScreenPressed = false;
            }

        }
    }

    private float AngleCalculator(Vector2 cubePoint)
    {

        Vector2 ReferencePoint = new Vector2(0, 1);

        float cosAngle = ((ReferencePoint.x * cubePoint.x) + (ReferencePoint.y * cubePoint.y)) /
            (Mathf.Sqrt(ReferencePoint.x * ReferencePoint.x + ReferencePoint.y * ReferencePoint.y) * (Mathf.Sqrt(cubePoint.x * cubePoint.x + cubePoint.y * cubePoint.y)));
        float angleRadian = Mathf.Asin(cosAngle);
        float angle = angleRadian * 180 / Mathf.PI;

        return angle;
    }

    public void ResetPosition()
    {

        // Stop la velocité du joueur
        rb.velocity = Vector2.zero;

        // Player au centre
        transform.position = new Vector2(0, 0);

        GetComponent<TrailRenderer>().time = 0;

        directionX = 0;
        directionY = 0;
    }

    private void ResetSleeve()
    {
        ResetPosition();

        // Met à faux les Flags
        isCollideRetargetDown = false;
        isCollideRetargetUp = false;
        isCollide = false;

        if (isScreenPressed)
        {
            canMove = false;
        }

        DOTween.Kill(transform);

        // Changement de couleur
        gameManager.ChangeColor();

        // Animation du player
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        transform.DOScaleX(2, 0.2f).SetEase(Ease.OutExpo);
        transform.DOScaleY(2, 0.2f).SetEase(Ease.OutExpo);

        if (isGetingPoint)
        {
            TargetAnimation();
        }


    }

    private void TargetAnimation()
    {
        isGetingPoint = false;
        // Animation des targets
        if (target.name == "CubeTargetUp" || target.name == "CubeTargetDown")
        {
            target.transform.localScale = new Vector3(
            targetsScaleXUp + 0.3f,
            targetsScaleYUp + 0.3f,
            targetsScaleZUp);
        }
        else if (target.name == "CubeTargetRight" || target.name == "CubeTargetLeft")
        {
            target.transform.localScale = new Vector3(
            targetsScaleXRight + 0.3f,
            targetsScaleYRight + 0.3f,
            targetsScaleZRight);
        }

        target.transform.DOScaleX(target.transform.localScale.x - 0.3f, 0.2f).SetEase(Ease.InExpo);
        target.transform.DOScaleY(target.transform.localScale.y - 0.3f, 0.2f).SetEase(Ease.InExpo);

        SpawnPulse();
    }

    private void SpawnPulse()
    {
        
        GameObject PulseCanvas = Instantiate(PulsePrefab);
        PulseCanvas.transform.position = target.transform.position;
        Transform Pulse = PulseCanvas.gameObject.transform.GetChild(0);

        Pulse.GetComponent<Image>().color = target.GetComponent<MeshRenderer>().material.color;
        
        Destroy(PulseCanvas, 0.5f);

        

        Vector2 PulseSize = new Vector2(
            Pulse.GetComponent<RectTransform>().sizeDelta.x,
            Pulse.GetComponent<RectTransform>().sizeDelta.y);

        Pulse.GetComponent<RectTransform>().DOSizeDelta(PulseSize * 1.4f, 0.5f).SetEase(Ease.OutExpo);
        Pulse.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.OutExpo);

    }

    private void LoosePointEffect()
    {
        losePoint.Play();
        CameraShakerHandler.Shake(MyShake);
        StartCoroutine(VibrateDuration());

        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1f);
        Color colorToChange = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0f);
        blackScreen.DOColor(colorToChange, 0.4f).SetEase(Ease.OutQuart);
    }

    void Update()
    {

        if (gameManager.canPlay)
        {
            MovePlayer();
        }  

        if (transform.position.x > 4.5f || transform.position.x < -4.5f || transform.position.y > 6.69f || transform.position.y < -6.69f)
        {
            transform.position = Vector2.zero;
        }

    }
}
