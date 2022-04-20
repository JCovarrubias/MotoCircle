using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public UnityEvent m_OnAddScore;
    private int score;

    public int Score
    {
        get { return score; }
        set { OnAddScore(value); }
    }

    private void Awake()
    {
        Instance = this;

        if (m_OnAddScore == null)
            m_OnAddScore = new UnityEvent();
    }

    private void Start()
    {
        GameManager.Instance.m_OnRestart.AddListener(ResetScore);
    }

    private void OnAddScore(int value)
    {
        score += value;
        m_OnAddScore?.Invoke();
    }

    private void ResetScore()
    {
        score = 0;
    }
}
