using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] GameObject wheel;
    [SerializeField] GameObject outsidePlayer;
    [SerializeField] GameObject insidePlayer;
    //[SerializeField] Button playerTap;

    GameManager gameManager;

    private float speed = 60f;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.m_OnRestart.AddListener(ResetPlayer);
    }

    private void Update()
    {
        if (gameManager.IsActive)
            wheel.transform.eulerAngles = new Vector3(0f, 0f, wheel.transform.eulerAngles.z + (speed * Time.deltaTime));
    }

    public void SwitchPlayer()
    {
        if (gameManager.IsActive)
        {
            outsidePlayer.SetActive(!outsidePlayer.activeSelf);
            insidePlayer.SetActive(!insidePlayer.activeSelf);
        }
        else if (gameManager.TutorialPlaying)
            gameManager.m_OnStart?.Invoke();
    }

    private void ResetPlayer()
    {
        wheel.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
