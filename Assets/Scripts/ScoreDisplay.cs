using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    private ScoreManager scoreManager;

    private TextMeshProUGUI score;

    private void OnEnable()
    {
        if (score != null)
            SetScore();
    }
    private void Start()
    {
        scoreManager = ScoreManager.Instance;
        scoreManager.m_OnAddScore.AddListener(SetScore);

        score = GetComponent<TextMeshProUGUI>();
    }

    private void SetScore()
    {
        score.text = scoreManager.Score.ToString();
    }
}
