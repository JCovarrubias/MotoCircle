using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class TruckBehaviour : MonoBehaviour
{
    [SerializeField] List<GameObject> wheels;
    [SerializeField] FingersMultiDragComponentScript drag;

    private GameManager gameManager;
    private GameObject truck;
    private float xPrevPos;
    private float speed;
    private float timeCount;

    public void Start()
    {
        gameManager = GameManager.Instance;
        truck = transform.parent.gameObject;
        xPrevPos = truck.transform.position.x;
        speed = 28f;
        timeCount = 0;

        gameManager.m_OnStart.AddListener(OnStartGame);
        gameManager.m_OnGameOver.AddListener(OnGameOver);
        gameManager.m_OnRestart.AddListener(OnRestartGame);
    }

    public void Update()
    {
        if (gameManager.IsActive)
        {
            timeCount += Time.deltaTime;
 
            if (timeCount >= 0.1f)
            {
                float tempSpeed = speed;
                if (truck.transform.position.x < xPrevPos)
                    tempSpeed = 14f;

                timeCount = 0;
                xPrevPos = truck.transform.position.x;

                foreach (GameObject wheel in wheels)
                {
                    wheel.transform.eulerAngles = wheel.transform.eulerAngles + new Vector3(0f, 0f, -tempSpeed);
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        ItemBehaviour item = collision.gameObject.GetComponent<ItemBehaviour>();
        if (item != null)
        {
            item.m_OnCollision?.Invoke();

            if (item.Type == ItemType.RIGHT)
                ScoreManager.Instance.Score = 1;
            else
                gameManager.m_OnGameOver?.Invoke();
        }
    }

    private void OnStartGame()
    {
        drag.DragX = true;
        timeCount = 0;
    }

    private void OnGameOver()
    {
        drag.DragX = false;
    }

    private void OnRestartGame()
    {
        truck.transform.position = new Vector2(0f, truck.transform.localPosition.y);
        xPrevPos = truck.transform.position.x;
    }
}
