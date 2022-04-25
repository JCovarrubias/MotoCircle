using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerBehaviour playerBehaviour;

    private void Start()
    {
        playerBehaviour = PlayerBehaviour.Instance;    
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
