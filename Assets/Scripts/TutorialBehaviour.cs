using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBehaviour : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.m_OnStart.AddListener(OnStartGame);
    }

    private void OnStartGame()
    {
        gameObject.SetActive(false);
    }
}
