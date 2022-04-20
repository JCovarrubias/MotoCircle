using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject gameplayUI;

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
        m_OnRestart.AddListener(OnResetGame);
    }

    private void OnStartGame()
    {
        IsActive = true;
        TutorialPlaying = false;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            DataManager.Instance.NotifyGameStart();
    }

    private void OnGameOver()
    {
        IsActive = false;
        Lives -= 1;
        dataManager.SendResults();
    }

    private void OnResetGame()
    {
        titleScreen.SetActive(true);
        gameplayUI.SetActive(false);
    }
}
