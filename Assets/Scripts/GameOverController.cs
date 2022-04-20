using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;

    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI lives;

    private GameManager gameManager;
    private ScoreManager scoreManager;

    private void OnEnable()
    {
        bestScore.text = scoreManager.Score.ToString();
        score.text = scoreManager.Score.ToString();
        lives.text = gameManager.Lives.ToString();
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;
        scoreManager = ScoreManager.Instance;

        gameManager.m_OnGameOver.AddListener(OnGameOver);
        gameManager.m_OnRestart.AddListener(OnResetGame);

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(() => gameManager.m_OnRestart?.Invoke());

        exitButton.onClick.RemoveAllListeners();
        //exitButton.onClick.AddListener(() => dataManager.m_OnExit?.Invoke());

        gameObject.SetActive(false);
    }

    private void OnGameOver()
    {
        gameObject.SetActive(true);
    }

    private void OnResetGame()
    {
        gameObject.SetActive(false);
    }
}
