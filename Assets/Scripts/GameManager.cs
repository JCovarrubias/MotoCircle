using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsActive { get; private set; }

    public int Lives { get; set; }

    public UnityEvent m_OnStart;
    public UnityEvent m_OnGameOver;
    public UnityEvent m_OnRestart;
    public UnityEvent m_OnClose;

    private void Awake()
    {
        Instance = this;

        if (m_OnStart == null)
            m_OnStart = new UnityEvent();

        m_OnStart.AddListener(StartGame);
        m_OnGameOver.AddListener(GameOver);
    }

    private void StartGame()
    {
        IsActive = true;
    }

    private void GameOver()
    {
        IsActive = false;
    }
}
