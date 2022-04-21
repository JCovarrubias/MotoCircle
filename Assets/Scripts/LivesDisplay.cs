using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
    private GameManager gameManager;

    private TextMeshProUGUI lives;

    private void OnEnable()
    {
        if (lives != null)
            SetLives();
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.m_OnStart.AddListener(SetLives);

        lives = GetComponent<TextMeshProUGUI>();
        SetLives();
    }

    private void SetLives()
    {
        lives.text = gameManager.Lives.ToString();
    }
}
