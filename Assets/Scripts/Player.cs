using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator dustAnimator; 

    PlayerBehaviour playerBehaviour;

    private void OnEnable()
    {
        if (GameManager.Instance.IsActive)
            dustAnimator.gameObject.SetActive(true);
    }

    private void Start()
    {
        playerBehaviour = PlayerBehaviour.Instance;
        GameManager.Instance.m_OnStart.AddListener(OnStartGame);
        GameManager.Instance.m_OnGameOver.AddListener(OnGameOver);
    }

    private void OnStartGame()
    {
        dustAnimator.gameObject.SetActive(true);
        dustAnimator.Play("dust");
    }

    private void OnGameOver()
    {
        dustAnimator.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        ItemBehaviour item = collision.gameObject.GetComponent<ItemBehaviour>();
        SpawnPoint spawnPoint = collision.gameObject.GetComponent<SpawnPoint>();
        if (item != null)
        {
            item.m_OnCollision?.Invoke();
            playerBehaviour.PlayCollisionSound(item.Type);

            if (item.Type == ItemType.RIGHT)
                ScoreManager.Instance.Score = 1;
            else
                GameManager.Instance.m_OnGameOver?.Invoke();
        }
        else if (spawnPoint != null)
        {
            playerBehaviour.PlayerIndex = spawnPoint.Index;
            if (playerBehaviour.PlayerIndex == 0 || playerBehaviour.PlayerIndex == 6)
            {
                playerBehaviour.m_OnPlayerAtIndex?.Invoke();
            }
        }
    }
}
