using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject livesPanel;
    [SerializeField] AudioSource failure;

    public static GameManager Instance;
    private DataManager dataManager;

    public bool IsActive { get; private set; }
    public bool TutorialPlaying { get; set; }
    public int Lives { get; set; }

    public UnityEvent m_OnStart;
    public UnityEvent m_OnGameOver;
    public UnityEvent m_OnRestart;

    private void Awake()
    {
        Instance = this;
        dataManager = DataManager.Instance;

        if (m_OnStart == null)
            m_OnStart = new UnityEvent();

        m_OnStart.AddListener(OnStartGame);
        m_OnGameOver.AddListener(OnGameOver);
        m_OnRestart.AddListener(InitGamePanels);
    }

    private void Start()
    {
        InitGamePanels();
    }

    private void OnStartGame()
    {
        if (!IsActive)
        {
            IsActive = true;
            TutorialPlaying = false;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                DataManager.Instance.NotifyGameStart();
        }
    }

    private void OnGameOver()
    {
        IsActive = false;
        dataManager.UserData.lives -= 1;
        Lives = dataManager.UserData.lives;
        failure.Play();
        dataManager.SendResults();
    }

    private void InitGamePanels()
    {
        titleScreen.SetActive(!(dataManager.UserData.lives <= 0));
        livesPanel.SetActive(dataManager.UserData.lives <= 0);
        gameplayUI.SetActive(false);
    }
}
