using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] GameObject wheel;
    [SerializeField] GameObject outsidePlayer;
    [SerializeField] GameObject insidePlayer;

    [SerializeField] AudioSource rightItemSound;
    [SerializeField] AudioSource wrongItemSound;
    [SerializeField] AudioSource tapSound;
    [SerializeField] AudioSource bikeSound;

    public UnityEvent m_OnPlayerAtIndex;

    public static PlayerBehaviour Instance;
    private GameManager gameManager;

    public int PlayerIndex
    {
        get; set;
    }

    private float speed;

    private void Awake()
    {
        Instance = this;

        if (m_OnPlayerAtIndex == null)
            m_OnPlayerAtIndex = new UnityEvent();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.m_OnRestart.AddListener(initPlayer);
        ScoreManager.Instance.m_OnAddScore.AddListener(AdjustPlayerSpeed);
        gameManager.m_OnGameOver.AddListener(() => bikeSound.Stop());
        initPlayer();
    }

    private void Update()
    {
        if (gameManager.IsActive)
        {
            float tempSpeed = outsidePlayer.activeSelf ? speed : speed * 1.25f;
            wheel.transform.eulerAngles = new Vector3(0f, 0f, wheel.transform.eulerAngles.z + (tempSpeed * Time.deltaTime));
        }
    }

    public void SwitchPlayer()
    {
        if (gameManager.IsActive)
        {
            tapSound.Play();
            outsidePlayer.SetActive(!outsidePlayer.activeSelf);
            insidePlayer.SetActive(!insidePlayer.activeSelf);
        }
        else if (gameManager.TutorialPlaying)
        {
            bikeSound.Play();
            gameManager.m_OnStart?.Invoke();
        }
    }

    private void initPlayer()
    {
        PlayerIndex = 0;
        speed = 80f;
        wheel.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        outsidePlayer.SetActive(true);
        insidePlayer.SetActive(false);
    }

    private void AdjustPlayerSpeed()
    {
        if (ScoreManager.Instance.Score % 5 == 0 && speed < 120f)
            speed += 10;
    }

    public void PlayCollisionSound(ItemType itemType)
    {
        if (itemType == ItemType.RIGHT)
            rightItemSound.Play();
        else
            wrongItemSound.Play();
    }
}
